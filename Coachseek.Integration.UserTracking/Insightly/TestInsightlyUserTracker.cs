using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace Coachseek.Integration.UserTracking.Insightly
{
    public class TestInsightlyUserTracker : InsightlyUserTracker
    {
        // login: olaf+testing@coachseek.com   password: coachseek#1
        private const string TEST_ACCOUNT_API_KEY = "2c5b7881-6da6-4ba0-8f35-d21d17e233ed";

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
