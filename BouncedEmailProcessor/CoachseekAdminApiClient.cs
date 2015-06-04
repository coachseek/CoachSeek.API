using System.Web;
using Coachseek.API.Client.Interfaces;

namespace BouncedEmailProcessor
{
    public class CoachseekAdminApiClient : ICoachseekAdminApiClient
    {
        private IAdminApiClient ApiClient { get; set; }

        public CoachseekAdminApiClient(IAdminApiClient apiClient)
        {
            ApiClient = apiClient;
        }

        public void UnsubscribeEmailAddress(string emailAddress)
        {
            var relativeUrl = string.Format("Email/Unsubscribe?email={0}", HttpUtility.UrlEncode(emailAddress));
            var response = ApiClient.Get<string>(relativeUrl);
        }
    }
}
