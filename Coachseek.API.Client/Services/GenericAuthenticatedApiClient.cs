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
            //SetBasicAuthRequestHeader(client);
        }

        //private void SetBasicAuthRequestHeader(HttpClient client)
        //{
        //    var authInfo = string.Format("{0}:{1}", Username, Password);
        //    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);
        //}
    }
}
