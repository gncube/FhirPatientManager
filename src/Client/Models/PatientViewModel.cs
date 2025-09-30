using System.ComponentModel.DataAnnotations;

namespace FhirPatientManager.Models;

public class PatientViewModel
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Family name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Family name must be between 2 and 100 characters")]
    public string FamilyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Given name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Given name must be between 2 and 100 characters")]
    public string GivenName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Gender is required")]
    public string Gender { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required")]
    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    [RegularExpression(@"^[\d\s\-\+\(\)]+$", ErrorMessage = "Phone number contains invalid characters")]
    public string PhoneNumber { get; set; } = string.Empty;

    public string FullName => $"{GivenName} {FamilyName}";
}
