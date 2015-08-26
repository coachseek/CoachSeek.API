using System;
using CoachSeek.Common.Extensions;
using Coachseek.Integration.Contracts.UserTracking;

namespace Coachseek.Integration.Tests.Unit.UserTracking.Fakes
{
    public class StubUserTrackerFactory : IUserTrackerFactory
    {
        public string Credentials { get; set; }
        public Exception Exception { set { UserTracker.Exception = value; } }
        public StubUserTracker UserTracker { get; private set; }


        public StubUserTrackerFactory()
        {
            UserTracker = new StubUserTracker();            
        }

        public IUserTracker GetUserTracker(bool isUserTrackingEnabled, bool isTesting)
        {
            return UserTracker;
        }
    }
}
