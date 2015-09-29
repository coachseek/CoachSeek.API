using System.Threading.Tasks;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;

namespace Coachseek.Integration.UserTracking.Insightly
{
    public class InsightlyApiWebClient : GenericAuthenticatedApiClient
    {
        public InsightlyApiWebClient(string credentials)
            : base("https://api.insight.ly/v2.1/", credentials, string.Empty)
        { }


        public ApiResponse PostLead(InsightlyLead lead)
        {
            return Post(lead, "Leads");
        }

        public async Task<ApiResponse> PostLeadAsync(InsightlyLead lead)
        {
            return await PostAsync(lead, "Leads");
        }


        private ApiResponse Post<TData>(TData data, string relativeUrl) where TData : InsightlyEntity
        {
            var json = JsonSerialiser.Serialise(data);
            return base.Post(json, relativeUrl);
        }

        private async Task<ApiResponse> PostAsync<TData>(TData data, string relativeUrl) where TData : InsightlyEntity
        {
            var json = JsonSerialiser.Serialise(data);
            return await base.PostAsync(json, relativeUrl);
        }
    }
}
