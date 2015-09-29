using System.Threading.Tasks;
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


        public async Task CreateTrackingUserAsync(UserData user, BusinessData business)
        {
            var lead = new InsightlyLead(user.FirstName, 
                                         user.LastName, 
                                         user.Email, 
                                         user.Phone, 
                                         business.Sport,
                                         business.Payment.Currency);

            await Client.PostLeadAsync(lead);
        }
    }
}
