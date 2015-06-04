using System.Net;
using Coachseek.API.Client.Models;

namespace Coachseek.API.Client.Services
{
    public class BusinessAnonymousApiClient : AnonymousApiClientBase
    {
        public ApiResponse Get<TResponse>(string businessDomain, string relativeUrl, string scheme = "https")
        {
            var http = CreateWebRequest(relativeUrl);
            SetBusinessDomainHeader(http, businessDomain);
            return Get<TResponse>(http);
        }

        public ApiResponse Post<TResponse>(string json, string businessDomain, string relativeUrl, string scheme = "https")
        {
            var http = CreateWebRequest(relativeUrl);
            SetBusinessDomainHeader(http, businessDomain);
            return Post<TResponse>(http, json);
        }

        private static void SetBusinessDomainHeader(WebRequest request, string domain)
        {
            if (domain != null)
                request.Headers["Business-Domain"] = domain;
        }
    }
}
