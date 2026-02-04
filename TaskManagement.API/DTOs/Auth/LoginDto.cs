using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.DTOs.Auth
{
    /// <summary>
    /// Request payload for user login
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// User email address
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        /// <summary>
        /// User password
        /// </summary>
        [Required]
        public string Password { get; set; } = null!;
    }
}
