using FhirPatientManager.Models;

namespace FhirPatientManager.Services;

public interface IFhirService
{
    Task<List<PatientViewModel>> GetAllPatientsAsync();
    Task<PatientViewModel?> GetPatientByIdAsync(string id);
    Task<PatientViewModel> CreatePatientAsync(PatientViewModel patient);
    Task<PatientViewModel> UpdatePatientAsync(PatientViewModel patient);
    Task<bool> DeletePatientAsync(string id);
    Task<List<PatientViewModel>> SearchPatientsByNameAsync(string searchTerm);
    Task<List<PatientViewModel>> SearchPatientsAsync(string searchTerm);
}
