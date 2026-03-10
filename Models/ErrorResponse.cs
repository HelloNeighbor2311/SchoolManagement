using System.Text.Json;
using System.Text.Json.Serialization;

namespace SchoolManagement.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string? Detail { get; set; }
        public List<string>? Errors { get; set; }
        public string? TraceId { get; set; }
        public DateTime TimeStamp { get; set; }

        public ErrorResponse()
        {
            TimeStamp = DateTime.UtcNow;
        }
        public ErrorResponse(int statusCode, string message, string? detail = null)
        {
            StatusCode = statusCode;
            Message = message;
            Detail = detail;
            TimeStamp = DateTime.UtcNow;
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true

            });

        }
    }
}
