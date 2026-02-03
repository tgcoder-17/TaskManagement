using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.DTOs.Auth
{
    public class RegisterDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MinLength(8)]
        public string Password { get; set; } = null!;

        public string? FullName { get; set; }
    }
}
