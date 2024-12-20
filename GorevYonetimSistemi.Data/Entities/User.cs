using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GorevYonetimSistemi.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace GorevYonetimSistemi.Data.Entities
{
    public class User
{
    [Key]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 50 characters.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Role is required.")]
    public RoleEnum Role { get; set; }

    [JsonIgnore]
    public ICollection<UserDuty> UserDuties { get; set; } = new List<UserDuty>();
}

}
