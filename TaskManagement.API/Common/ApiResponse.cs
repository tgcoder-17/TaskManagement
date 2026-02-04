namespace TaskManagement.API.Common
{

    /// <summary>
    /// Standard wrapper for successful API responses to ensure
    /// consistent response structure across the application.
    /// </summary>
    /// <typeparam name="T">Type of the response data</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// HTTP status code of the response
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Human-readable message describing the result
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Response payload data
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Creates a successful API response with data
        /// </summary>
        /// <param name="data">Response payload</param>
        /// <param name="statusCode">HTTP status code (default: 200)</param>
        /// <param name="message">Optional success message</param>
        /// <returns>Wrapped successful API response</returns>
        public static ApiResponse<T> Success(
            T data,
            int statusCode = StatusCodes.Status200OK,
            string message = "Success")
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// Creates a successful API response without a response body
        /// </summary>
        /// <param name="message">Optional success message</param>
        /// <returns>Wrapped no-content API response</returns>
        public static ApiResponse<T> NoContent(
            string message = "Operation successful")
        {
            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status204NoContent,
                Message = message
            };
        }
    }
}
