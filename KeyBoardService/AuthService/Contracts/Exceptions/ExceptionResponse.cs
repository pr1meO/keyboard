using System.Net;

namespace AuthService.API.Contracts.Exceptions
{
    public class ExceptionResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
