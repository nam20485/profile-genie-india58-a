using System.ComponentModel.DataAnnotations;

namespace ProfileGenie.Shared.DTOs;

/// <summary>
/// DTO for creating a new profile.
/// </summary>
public class CreateProfileDto
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
