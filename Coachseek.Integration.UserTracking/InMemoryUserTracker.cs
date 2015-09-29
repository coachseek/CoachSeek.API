using System.Threading.Tasks;
using CoachSeek.Data.Model;
using Coachseek.Integration.Contracts.UserTracking;

namespace Coachseek.Integration.UserTracking
{
    public class InMemoryUserTracker : IUserTracker
    {
        public bool WasCreateUserCalled;
        public UserData User;

        public string Credentials { get; private set; }

        public InMemoryUserTracker(string credentials)
        {
            Credentials = credentials;
        }

        public void CreateTrackingUser(UserData user, BusinessData business)
        {
            WasCreateUserCalled = true;
        }

        public async Task CreateTrackingUserAsync(UserData user, BusinessData business)
        {
            WasCreateUserCalled = true;

            await Task.Delay(1000);
        }
    }
}
