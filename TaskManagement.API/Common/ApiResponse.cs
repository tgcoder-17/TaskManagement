namespace TaskManagement.API.Common
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

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
