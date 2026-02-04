using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.DTOs.Auth
{
    /// <summary>
    /// Request payload for user registration
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// User email address (must be unique)
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Password must be at least 8 characters long,
        /// contain at least one uppercase letter and one number
        /// </summary>
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(
        @"^(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "Password must contain at least one uppercase letter and one number")]
        public string Password { get; set; } = null!;

        /// <summary>
        /// Full name of the user
        /// </summary>
        public string? FullName { get; set; }
    }
}
