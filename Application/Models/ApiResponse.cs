using System.Net;

namespace Application.Models
{
    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public bool Success { get; set; }
        public T? Data { get; set; } = default!;

        public ApiResponse(HttpStatusCode statusCode, string message, bool success, T data)
        {
            StatusCode = statusCode;
            Message = message;
            Success = success;
            Data = data;
        }

        public ApiResponse(HttpStatusCode statusCode, string message, bool success)
        {
            StatusCode = statusCode;
            Message = message;
            Success = success;
        }

        public ApiResponse(HttpStatusCode statusCode, bool success, T data)
        {
            StatusCode = statusCode;
            Success = success;
            Data = data;
        }
    }
}
