using FhirPatientManager.Client.Models;

namespace FhirPatientManager.Client.Services;

/// <summary>
/// Service contract for FHIR patient operations.
/// </summary>
public interface IFhirService
{
    /// <summary>
    /// Retrieves all patients from the FHIR server.
    /// </summary>
    Task<List<PatientViewModel>> GetAllPatientsAsync();

    /// <summary>
    /// Retrieves a specific patient by ID.
    /// </summary>
    Task<PatientViewModel?> GetPatientByIdAsync(string id);

    /// <summary>
    /// Searches for patients matching the search term.
    /// </summary>
    Task<List<PatientViewModel>> SearchPatientsAsync(string searchTerm);

    /// <summary>
    /// Creates a new patient in the FHIR server.
    /// </summary>
    Task<PatientViewModel> CreatePatientAsync(PatientViewModel patient);

    /// <summary>
    /// Updates an existing patient in the FHIR server.
    /// </summary>
    Task<PatientViewModel> UpdatePatientAsync(PatientViewModel patient);

    /// <summary>
    /// Deletes a patient from the FHIR server.
    /// </summary>
    Task<bool> DeletePatientAsync(string id);
}
