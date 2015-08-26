using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;

namespace Coachseek.Integration.UserTracking.Insightly
{
    public class InsightlyApiWebClient : BasicAuthenticationApiWebClient
    {
        private string Credentials { get; set; }

        public InsightlyApiWebClient(string credentials)
            : base("https://api.insight.ly/v2.1")
        {
            Credentials = credentials;
        }

        private string Username { get { return Credentials; } }
        private string Password { get { return string.Empty; } }


        public ApiResponse PostLead<TLead>(TLead lead)
        {
            return Post(lead, "Leads");
        }


        private ApiResponse Post<TData>(TData data, string relativeUrl)
        {
            var json = JsonSerialiser.Serialise(data);
            return Post(json, Username, Password, relativeUrl);
        }
    }
}
