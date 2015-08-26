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
            var lead = new InsightlyLead(user.FirstName, user.LastName, user.Email);

            Client.PostLead(lead);




            //Service.CreateContact(new Crm.Contact(user.Name)
            //{
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    ContactInfos = new List<Crm.ContactInfo>(new[]{
            //            new Crm.ContactInfo{
            //                ContactType = "Email",
            //                Detail = user.Email
            //            }
            //        })
            //});
        }
    }
}
