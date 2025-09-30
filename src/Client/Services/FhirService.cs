using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using FhirPatientManager.Client.Models;

namespace FhirPatientManager.Client.Services;

/// <summary>
/// Implementation of FHIR patient operations using the HAPI FHIR public server.
/// Connects to https://hapi.fhir.org/baseR4 for real FHIR R4 operations.
/// </summary>
public class FhirService : IFhirService
{
    private readonly FhirClient _fhirClient;
    private const string FhirServerUrl = "https://hapi.fhir.org/baseR4";

    public FhirService()
    {
        _fhirClient = new FhirClient(FhirServerUrl)
        {
            Settings = new FhirClientSettings
            {
                PreferredFormat = ResourceFormat.Json,
                Timeout = 60 * 1000 // 60 seconds
            }
        };
    }

    public async Task<List<PatientViewModel>> GetAllPatientsAsync()
    {
        try
        {
            var searchParams = new SearchParams().LimitTo(20);
            var bundle = await _fhirClient.SearchAsync<Patient>(searchParams);
            var patients = bundle?.Entry?
                .Select(entry => entry.Resource as Patient)
                .Where(p => p != null)
                .Select(p => MapToViewModel(p!))
                .ToList() ?? new List<PatientViewModel>();
            return patients;
        }
        catch (Exception)
        {
            return new List<PatientViewModel>();
        }
    }

    public async Task<PatientViewModel?> GetPatientByIdAsync(string id)
    {
        try
        {
            var patient = await _fhirClient.ReadAsync<Patient>($"Patient/{id}");
            return patient != null ? MapToViewModel(patient) : null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<List<PatientViewModel>> SearchPatientsAsync(string searchTerm)
    {
        try
        {
            var searchParams = new SearchParams()
                .Where($"name:contains={searchTerm}")
                .LimitTo(50);
            var bundle = await _fhirClient.SearchAsync<Patient>(searchParams);
            var patients = bundle?.Entry?
                .Select(entry => entry.Resource as Patient)
                .Where(p => p != null)
                .Select(p => MapToViewModel(p!))
                .ToList() ?? new List<PatientViewModel>();
            return patients;
        }
        catch (Exception)
        {
            return new List<PatientViewModel>();
        }
    }

    public async Task<PatientViewModel> CreatePatientAsync(PatientViewModel patientViewModel)
    {
        try
        {
            var patient = MapToFhirPatient(patientViewModel);
            var createdPatient = await _fhirClient.CreateAsync(patient);
            return createdPatient != null ? MapToViewModel(createdPatient) : patientViewModel;
        }
        catch (Exception)
        {
            return patientViewModel;
        }
    }

    public async Task<PatientViewModel> UpdatePatientAsync(PatientViewModel patientViewModel)
    {
        try
        {
            var patient = MapToFhirPatient(patientViewModel);
            patient.Id = patientViewModel.Id;
            var updatedPatient = await _fhirClient.UpdateAsync(patient);
            return updatedPatient != null ? MapToViewModel(updatedPatient) : patientViewModel;
        }
        catch (Exception)
        {
            return patientViewModel;
        }
    }

    public async Task<bool> DeletePatientAsync(string id)
    {
        try
        {
            await _fhirClient.DeleteAsync($"Patient/{id}");
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private PatientViewModel MapToViewModel(Patient patient)
    {
        var givenName = patient.Name?.FirstOrDefault()?.Given?.FirstOrDefault() ?? string.Empty;
        var familyName = patient.Name?.FirstOrDefault()?.Family ?? string.Empty;
        var phoneNumber = patient.Telecom?.FirstOrDefault(t => t.System == ContactPoint.ContactPointSystem.Phone)?.Value;
        var gender = patient.Gender?.ToString() ?? "Unknown";
        var birthDate = !string.IsNullOrEmpty(patient.BirthDate) ? DateTime.Parse(patient.BirthDate) : (DateTime?)null;

        return new PatientViewModel
        {
            Id = patient.Id,
            GivenName = givenName,
            FamilyName = familyName,
            Gender = gender,
            BirthDate = birthDate,
            PhoneNumber = phoneNumber
        };
    }

    private Patient MapToFhirPatient(PatientViewModel viewModel)
    {
        var patient = new Patient
        {
            Name = new List<HumanName>
            {
                new HumanName
                {
                    Given = new[] { viewModel.GivenName },
                    Family = viewModel.FamilyName
                }
            },
            Gender = Enum.TryParse<AdministrativeGender>(viewModel.Gender, true, out var gender)
                ? gender
                : AdministrativeGender.Unknown,
            BirthDate = viewModel.BirthDate?.ToString("yyyy-MM-dd")
        };

        if (!string.IsNullOrWhiteSpace(viewModel.PhoneNumber))
        {
            patient.Telecom = new List<ContactPoint>
            {
                new ContactPoint
                {
                    System = ContactPoint.ContactPointSystem.Phone,
                    Value = viewModel.PhoneNumber,
                    Use = ContactPoint.ContactPointUse.Mobile
                }
            };
        }

        return patient;
    }
}
