using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using FhirPatientManager.Models;

namespace FhirPatientManager.Services;

public class FhirService : IFhirService
{
    private readonly FhirClient _client;
    private const string FhirServerUrl = "https://hapi.fhir.org/baseR4";

    public FhirService()
    {
        _client = new FhirClient(FhirServerUrl)
        {
            Settings = new FhirClientSettings
            {
                PreferredFormat = ResourceFormat.Json,
                Timeout = 30000
            }
        };
    }

    public async Task<List<PatientViewModel>> GetAllPatientsAsync()
    {
        try
        {
            var bundle = await _client.SearchAsync<Patient>(new[] { "_count=50" });
            return bundle.Entry.Select(e => MapToViewModel((Patient)e.Resource)).ToList();
        }
        catch
        {
            return new List<PatientViewModel>();
        }
    }

    public async Task<PatientViewModel?> GetPatientByIdAsync(string id)
    {
        try
        {
            var patient = await _client.ReadAsync<Patient>($"Patient/{id}");
            return MapToViewModel(patient);
        }
        catch
        {
            return null;
        }
    }

    public async Task<PatientViewModel> CreatePatientAsync(PatientViewModel patientVm)
    {
        var patient = MapToFhirPatient(patientVm);
        var created = await _client.CreateAsync(patient);
        return MapToViewModel(created);
    }

    public async Task<PatientViewModel> UpdatePatientAsync(PatientViewModel patientVm)
    {
        var patient = MapToFhirPatient(patientVm);
        patient.Id = patientVm.Id;
        var updated = await _client.UpdateAsync(patient);
        return MapToViewModel(updated);
    }

    public async Task<bool> DeletePatientAsync(string id)
    {
        try
        {
            await _client.DeleteAsync($"Patient/{id}");
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<PatientViewModel>> SearchPatientsByNameAsync(string searchTerm)
    {
        try
        {
            var bundle = await _client.SearchAsync<Patient>(new[]
            {
                $"name:contains={searchTerm}",
                "_count=50"
            });
            return bundle.Entry.Select(e => MapToViewModel((Patient)e.Resource)).ToList();
        }
        catch
        {
            return new List<PatientViewModel>();
        }
    }

    public async Task<List<PatientViewModel>> SearchPatientsAsync(string searchTerm)
    {
        try
        {
            // Search by name
            var nameBundle = await _client.SearchAsync<Patient>(new[]
            {
                $"name:contains={searchTerm}",
                "_count=50"
            });

            // Search by phone
            var phoneBundle = await _client.SearchAsync<Patient>(new[]
            {
                $"telecom={searchTerm}",
                "_count=50"
            });

            var patients = new Dictionary<string, PatientViewModel>();

            foreach (var entry in nameBundle.Entry.Concat(phoneBundle.Entry))
            {
                var patient = (Patient)entry.Resource;
                if (!patients.ContainsKey(patient.Id))
                {
                    patients[patient.Id] = MapToViewModel(patient);
                }
            }

            return patients.Values.ToList();
        }
        catch
        {
            return new List<PatientViewModel>();
        }
    }

    private PatientViewModel MapToViewModel(Patient patient)
    {
        var name = patient.Name.FirstOrDefault();
        var phone = patient.Telecom?.FirstOrDefault(t => t.System == ContactPoint.ContactPointSystem.Phone);

        return new PatientViewModel
        {
            Id = patient.Id,
            FamilyName = name?.Family ?? string.Empty,
            GivenName = name?.Given.FirstOrDefault() ?? string.Empty,
            Gender = patient.Gender?.ToString() ?? string.Empty,
            BirthDate = DateTime.TryParse(patient.BirthDate, out var date) ? date : null,
            PhoneNumber = phone?.Value ?? string.Empty
        };
    }

    private Patient MapToFhirPatient(PatientViewModel vm)
    {
        return new Patient
        {
            Name = new List<HumanName>
            {
                new HumanName
                {
                    Family = vm.FamilyName,
                    Given = new[] { vm.GivenName }
                }
            },
            Gender = Enum.TryParse<AdministrativeGender>(vm.Gender, true, out var gender)
                ? gender
                : AdministrativeGender.Unknown,
            BirthDate = vm.BirthDate?.ToString("yyyy-MM-dd"),
            Telecom = new List<ContactPoint>
            {
                new ContactPoint
                {
                    System = ContactPoint.ContactPointSystem.Phone,
                    Value = vm.PhoneNumber,
                    Use = ContactPoint.ContactPointUse.Mobile
                }
            }
        };
    }
}
