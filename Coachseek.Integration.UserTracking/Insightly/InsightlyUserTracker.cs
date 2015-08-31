using CoachSeek.Data.Model;
using Coachseek.Integration.Contracts.UserTracking;

namespace Coachseek.Integration.UserTracking.Insightly
{
    public class InsightlyUserTracker : IUserTracker
    {
        private InsightlyApiWebClient Client { get; set; }

        public InsightlyUserTracker(string credentials)
        {
            Client = new InsightlyApiWebClient(credentials);
        }


        public void CreateTrackingUser(UserData user)
        {
            var lead = new InsightlyLead(user.FirstName, user.LastName, user.Email, user.Phone);

            Client.PostLead(lead);
        }
    }
}
