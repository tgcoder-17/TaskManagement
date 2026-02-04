namespace TaskManagement.API.DTOs.Auth
{
    /// <summary>
    /// Response payload containing JWT token and user information
    /// </summary>
    public class AuthResponseDto
    {
        /// <summary>
        /// JWT access token
        /// </summary>
        public string Token { get; set; } = null!;
        /// <summary>
        /// Unique identifier of the authenticated user
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Token expiration timestamp
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
}
