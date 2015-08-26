using System;
using System.Net;
using System.Text;
using Coachseek.API.Client.Models;
using CoachSeek.Common;

namespace Coachseek.API.Client.Services
{
    public abstract class BasicAuthenticationApiWebClient : ApiWebClient
    {
        protected BasicAuthenticationApiWebClient(string baseUrl, ApiDataFormat dataFormat = ApiDataFormat.Json)
            : base(baseUrl, dataFormat)
        { }

        protected BasicAuthenticationApiWebClient(string scheme, string host, int? port = null, ApiDataFormat dataFormat = ApiDataFormat.Json)
            : base(scheme, host, port, dataFormat)
        { }


        public ApiResponse Get<TResponse>(string username, string password, string relativeUrl)
        {
            var http = CreateWebRequest(relativeUrl);
            SetBasicAuthHeader(http, username, password);
            return Get<TResponse>(http);
        }

        public ApiResponse Post(string json, string username, string password, string relativeUrl)
        {
            var http = CreateWebRequest(relativeUrl);
            SetBasicAuthHeader(http, username, password);
            return Post(http, json);
        }

        public ApiResponse Delete(string username, string password, string relativeUrl, string id)
        {
            var http = CreateWebRequest(relativeUrl, id);
            SetBasicAuthHeader(http, username, password);
            return Delete(http);
        }


        private void SetBasicAuthHeader(WebRequest request, string username, string password)
        {
            var authInfo = string.Format("{0}:{1}", username, password);
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
        }
    }
}
