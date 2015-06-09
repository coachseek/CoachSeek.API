using Coachseek.API.Client.Models;

namespace Coachseek.API.Client.Services
{
    public class AnonymousApiClient : AnonymousApiClientBase
    {
        public ApiResponse Get<TResponse>(string relativeUrl, string scheme = "https")
        {
            var http = CreateWebRequest(relativeUrl, scheme);
            return Get<TResponse>(http);
        }

        public ApiResponse Post<TResponse>(string json, string relativeUrl, string scheme = "https")
        {
            var http = CreateWebRequest(relativeUrl, scheme);
            return Post<TResponse>(http, json);
        }
    }
}
