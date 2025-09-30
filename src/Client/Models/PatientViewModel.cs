using System.ComponentModel.DataAnnotations;

namespace FhirPatientManager.Client.Models;

/// <summary>
/// View model representing a FHIR patient for UI operations.
/// </summary>
public class PatientViewModel
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Given name is required")]
    [StringLength(50, ErrorMessage = "Given name cannot exceed 50 characters")]
    public string GivenName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Family name is required")]
    [StringLength(50, ErrorMessage = "Family name cannot exceed 50 characters")]
    public string FamilyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Gender is required")]
    public string Gender { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required")]
    public DateTime? BirthDate { get; set; }

    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets the full name combining given and family names.
    /// </summary>
    public string FullName => $"{GivenName} {FamilyName}";
}
