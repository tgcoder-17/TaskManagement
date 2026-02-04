namespace TaskManagement.API.Common
{
    /// <summary>
    /// Standard response model used for returning error details
    /// in a consistent format across the API.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// HTTP status code associated with the error
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// High-level error message describing the failure
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Collection of detailed error messages
        /// </summary>
        public List<string> Errors { get; set; } = new();
    }

}
