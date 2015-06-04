using System.Net;

namespace Coachseek.API.Client.Models
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; private set; }
        public object Payload { get; private set; }

        public ApiResponse(HttpStatusCode statusCode, object payload = null)
        {
            StatusCode = statusCode;
            Payload = payload;
        }
    }
}
