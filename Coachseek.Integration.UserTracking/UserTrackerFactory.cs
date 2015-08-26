using Coachseek.Integration.Contracts.UserTracking;
using Coachseek.Integration.UserTracking.Insightly;

namespace Coachseek.Integration.UserTracking
{
    public class UserTrackerFactory : IUserTrackerFactory
    {
        public string Credentials { set; private get; }

        public IUserTracker GetUserTracker(bool isUserTrackingEnabled, bool isTesting)
        {
            if (!isUserTrackingEnabled)
                return new DoNothingUserTracker(Credentials);
            if (isTesting)
                return new InMemoryUserTracker(Credentials);

            return new InsightlyUserTracker(Credentials);
        }
    }
}
