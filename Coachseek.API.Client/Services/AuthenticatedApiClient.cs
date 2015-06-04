using Coachseek.API.Client.Models;

namespace Coachseek.API.Client.Services
{
    public class AuthenticatedApiClient : AuthenticatedApiClientBase
    {
        public ApiResponse Get<TResponse>(string username, string password, string relativeUrl, string scheme = "https")
        {
            var http = CreateWebRequest(relativeUrl, scheme);
            SetBasicAuthHeader(http, username, password);
            return Get<TResponse>(http);
        }

        public ApiResponse Post<TResponse>(string json, string username, string password, string relativeUrl, string scheme = "https")
        {
            var http = CreateWebRequest(relativeUrl, scheme);
            SetBasicAuthHeader(http, username, password);
            return Post<TResponse>(http, json);
        }
    }
}
