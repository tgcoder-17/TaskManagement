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
        /// User password (minimum 8 characters)
        /// </summary>
        [Required, MinLength(8)]
        public string Password { get; set; } = null!;

        /// <summary>
        /// Full name of the user
        /// </summary>
        public string? FullName { get; set; }
    }
}
