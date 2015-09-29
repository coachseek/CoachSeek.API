using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace Coachseek.Integration.UserTracking.Insightly
{
    public class TestInsightlyUserTracker : InsightlyUserTracker
    {
        private const string TEST_ACCOUNT_API_KEY = "99e5acda-2333-40e2-8771-02335451206c";

        public TestInsightlyUserTracker() 
            : base(TEST_ACCOUNT_API_KEY)
        { }


        public override void CreateTrackingUser(UserData user, BusinessData business)
        {
            var lead = new TestInsightlyLead(user, business);
            Client.PostLead(lead);
        }

        public override async Task CreateTrackingUserAsync(UserData user, BusinessData business)
        {
            var lead = new TestInsightlyLead(user, business);
            await Client.PostLeadAsync(lead);
        }
    }
}
