using System;
using System.Net;
using System.Text;
using CoachSeek.Common;

namespace Coachseek.API.Client.Services
{
    public class CoachseekAuthenticatedApiClient : CoachseekAnonymousApiClient
    {
        private string Username { get; set; }
        private string Password { get; set; }

        protected CoachseekAuthenticatedApiClient(string username,
                                                  string password,
                                                  string scheme = "https",
                                                  ApiDataFormat dataFormat = ApiDataFormat.Json)
            : base(scheme, dataFormat)
        {
            Username = username;
            Password = password;
        }


        protected override void ModifyRequest(HttpWebRequest request)
        {
            SetBasicAuthHeader(request);
        }


        private void SetBasicAuthHeader(WebRequest request)
        {
            var authInfo = string.Format("{0}:{1}", Username, Password);
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
        }
    }
}
