using System;
using Coachseek.API.Client.Interfaces;
using Coachseek.API.Client.Models;

namespace Coachseek.API.Client.Services
{
    public class AdminApiClient : AuthenticatedApiClientBase, IAdminApiClient
    {
        const string AdminUserName = "userZvFXUEmjht1hFJGn+H0YowMqO+5u5tEI";
        const string AdminPassword = "passYBoVaaWVp1W9ywZOHK6E6QXFh3z3+OUf";


        public ApiResponse Get<TResponse>(string relativeUrl, string scheme = "https")
        {
            var http = CreateWebRequest(relativeUrl, scheme);
            SetBasicAuthHeader(http, AdminUserName, AdminPassword);
            return Get<TResponse>(http);
        }

        protected override Uri FormatUrl(string relativeUrl, string scheme = "https")
        {
            return new Uri(string.Format("{0}/Admin/{1}", FormatBaseUrl(scheme), relativeUrl));
        }
    }
}
