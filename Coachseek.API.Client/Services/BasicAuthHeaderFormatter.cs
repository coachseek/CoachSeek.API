using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Coachseek.API.Client.Services
{
    public static class BasicAuthHeaderSetter
    {
        public static void SetBasicAuthHeader(WebRequest request, string username, string password)
        {
            request.Headers["Authorization"] = CalculateAuthHeaderValue(username, password);
        }

        public static void SetBasicAuthHeader(HttpClient client, string username, string password)
        {
            var authHeader = CalculateAuthHeaderValue(username, password);
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authHeader);
        }


        private static string CalculateAuthHeaderValue(string username, string password)
        {
            var authInfo = string.Format("{0}:{1}", username, password);
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            return string.Format("Basic {0}", authInfo);
        }
    }
}
