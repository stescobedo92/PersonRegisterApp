using System.ComponentModel.DataAnnotations;

namespace PersonRegisterApp.Web.Models;

public class Person
{
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [RegularExpression(@"^\+?[\d\s\-\(\)]{7,20}$", ErrorMessage = "Phone number format is invalid.")]
    public string PhoneNumber { get; set; } = string.Empty;
}
