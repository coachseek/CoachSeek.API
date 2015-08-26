using System.Collections.Generic;
using CoachSeek.Data.Model;
using Coachseek.Integration.Contracts.UserTracking;
using Crm = Insightly;

namespace Coachseek.Integration.UserTracking.Insightly
{
    public class InsightlyUserTracker : IUserTracker
    {
        private string Credentials { set; get; }


        public InsightlyUserTracker(string credentials)
        {
            Credentials = credentials;
        }


        private Crm.InsightlyService Service
        {
            get { return new Crm.InsightlyService(Credentials); }
        }


        public void CreateTrackingUser(UserData user)
        {





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


        private void CreateInsightlyContact()
        {
        }
    }
}
