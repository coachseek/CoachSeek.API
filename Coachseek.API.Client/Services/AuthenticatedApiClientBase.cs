using System;
using System.Net;
using System.Text;

namespace Coachseek.API.Client.Services
{
    public abstract class AuthenticatedApiClientBase : ApiClientBase
    {
        protected void SetBasicAuthHeader(WebRequest request, string username, string password)
        {
            var authInfo = string.Format("{0}:{1}", username, password);
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
        }
    }
}
