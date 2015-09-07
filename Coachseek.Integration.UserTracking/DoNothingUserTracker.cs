using CoachSeek.Data.Model;
using Coachseek.Integration.Contracts.UserTracking;

namespace Coachseek.Integration.UserTracking
{
    public class DoNothingUserTracker : IUserTracker
    {
        public DoNothingUserTracker(string credentials)
        {
            
        }

        public void CreateTrackingUser(UserData user, BusinessData business)
        {
            // Do nothing
        }
    }
}
