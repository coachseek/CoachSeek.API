using System.Net;

namespace Coachseek.API.Client
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; private set; }
        public object Payload { get; private set; }

        public Response(HttpStatusCode statusCode, object payload = null)
        {
            StatusCode = statusCode;
            Payload = payload;
        }
    }
}
