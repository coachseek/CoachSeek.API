using System.Net;
using System.Net.Http;
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
            BasicAuthHeaderSetter.SetBasicAuthHeader(request, Username, Password);
        }

        protected override void SetOtherRequestHeaders(HttpRequestMessage request)
        {
            base.SetOtherRequestHeaders(request);
            BasicAuthHeaderSetter.SetBasicAuthHeader(request, Username, Password);
        }
    }
}
