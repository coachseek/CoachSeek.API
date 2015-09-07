using System;
using CoachSeek.Data.Model;
using Coachseek.Integration.Contracts.UserTracking;

namespace Coachseek.Integration.Tests.Unit.UserTracking.Fakes
{
    public class StubUserTracker : IUserTracker
    {
        public bool WasCreateTrackingUserCalled;
        public Exception Exception;

        public UserData User { get; private set; }

        public void CreateTrackingUser(UserData user, BusinessData business)
        {
            WasCreateTrackingUserCalled = true;
            User = user;

            if (Exception != null)
                throw Exception;
        }
    }
}
