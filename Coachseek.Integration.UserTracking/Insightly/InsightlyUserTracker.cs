using System.Threading.Tasks;
using CoachSeek.Data.Model;
using Coachseek.Integration.Contracts.UserTracking;

namespace Coachseek.Integration.UserTracking.Insightly
{
    public class InsightlyUserTracker : IUserTracker
    {
        protected InsightlyApiWebClient Client { get; set; }

        public InsightlyUserTracker(string credentials)
        {
            Client = new InsightlyApiWebClient(credentials);
        }


        public virtual void CreateTrackingUser(UserData user, BusinessData business)
        {
            var lead = new InsightlyLead(user, business);
            Client.PostLead(lead);
        }

        public virtual async Task CreateTrackingUserAsync(UserData user, BusinessData business)
        {
            var lead = new InsightlyLead(user, business);
            await Client.PostLeadAsync(lead);
        }
    }
}
