using System.Net;
using System.Net.Http;
using CoachSeek.Common;

namespace Coachseek.API.Client.Services
{
    public abstract class GenericAuthenticatedApiClient : GenericApiClient
    {
        private string Username { get; set; }
        private string Password { get; set; }

        protected GenericAuthenticatedApiClient(string baseUrl,
                                                string username,
                                                string password,
                                                ApiDataFormat dataFormat = ApiDataFormat.Json)
            : base(baseUrl, dataFormat)
        {
            Username = username;
            Password = password;
        }


        protected override void ModifyRequest(HttpWebRequest request)
        {
            BasicAuthHeaderSetter.SetBasicAuthHeader(request, Username, Password);
        }

        protected override void SetOtherRequestHeaders(HttpClient client)
        {
            BasicAuthHeaderSetter.SetBasicAuthHeader(client, Username, Password);
        }
    }
}
