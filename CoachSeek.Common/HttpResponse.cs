using System.Net;

namespace CoachSeek.Common
{
    public class HttpResponse
    {
        public HttpStatusCode StatusCode { get; private set; }
        public object Payload { get; private set; }

        public HttpResponse(HttpStatusCode statusCode, object payload = null)
        {
            StatusCode = statusCode;
            Payload = payload;
        }
    }
}
