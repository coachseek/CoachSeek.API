using Coachseek.Integration.Contracts.UserTracking;
using Coachseek.Integration.UserTracking;
using Coachseek.Integration.UserTracking.Insightly;
using NUnit.Framework;

namespace Coachseek.Integration.Tests.Unit.UserTracking.Tests
{
    [TestFixture]
    public class UserTrackerFactoryTests
    {
        [Test]
        public void GivenUserTrackingIsNotEnabled_WhenCallGetUserTracker_ThenReturnNullUserTracker()
        {
            var request = GivenUserTrackingIsNotEnabled();
            var response = WhenCallGetUserTracker(request);
            ThenReturnNullUserTracker(response);
        }

        [Test]
        public void GivenIsTestingIsTrue_WhenCallGetUserTracker_ThenReturnInMemoryUserTracker()
        {
            var request = GivenIsTestingIsTrue();
            var response = WhenCallGetUserTracker(request);
            ThenReturnInMemoryUserTracker(response);
        }

        [Test]
        public void GivenIsTestingIsFalse_WhenCallGetUserTracker_ThenReturnInsightlyUserTracker()
        {
            var request = GivenIsTestingIsFalse();
            var response = WhenCallGetUserTracker(request);
            ThenReturnInsightlyUserTracker(response);
        }


        private Parameters GivenUserTrackingIsNotEnabled()
        {
            return new Parameters
            {
                IsUserTrackingEnabled = false
            };
        }

        private Parameters GivenIsTestingIsTrue()
        {
            return new Parameters
            {
                IsUserTrackingEnabled = true,
                IsTesting = true
            };
        }

        private Parameters GivenIsTestingIsFalse()
        {
            return new Parameters
            {
                IsUserTrackingEnabled = true,
                IsTesting = false
            };
        }


        private IUserTracker WhenCallGetUserTracker(Parameters parameters)
        {
            var factory = new UserTrackerFactory();
            return factory.GetUserTracker(parameters.IsUserTrackingEnabled, parameters.IsTesting);
        }


        private void ThenReturnNullUserTracker(IUserTracker userTracker)
        {
            Assert.That(userTracker, Is.InstanceOf<DoNothingUserTracker>());
        }

        private void ThenReturnInMemoryUserTracker(IUserTracker userTracker)
        {
            Assert.That(userTracker, Is.InstanceOf<InMemoryUserTracker>());
        }

        private void ThenReturnInsightlyUserTracker(IUserTracker userTracker)
        {
            Assert.That(userTracker, Is.InstanceOf<InsightlyUserTracker>());
        }


        private struct Parameters
        {
            public bool IsUserTrackingEnabled;
            public bool IsTesting;
        }
    }
}
