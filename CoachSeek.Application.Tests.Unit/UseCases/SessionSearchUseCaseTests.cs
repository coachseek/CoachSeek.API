using System.Collections.Generic;
using System.Linq;
using CoachSeek.Application.UseCases;
using CoachSeek.Data.Model;
using NUnit.Framework;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class SessionSearchUseCaseTests : UseCaseTests
    {
        [SetUp]
        public void Setup()
        {
            SetupBusinessRepository();
        }


        [Test]
        public void GivenNoStartDate_WhenCallSearchForSessions_ThenSessionSearchFailsWithInvalidStartDateError()
        {
            var useCase = new SessionSearchUseCase(null);
            var response = useCase.SearchForSessions(null, "2015-01-20");
            AssertSingleError(response, "Invalid start date.");
        }

        [Test]
        public void GivenNoValidStartDate_WhenCallSearchForSessions_ThenSessionSearchFailsWithInvalidStartDateError()
        {
            var useCase = new SessionSearchUseCase(null);
            var response = useCase.SearchForSessions("abc", "2015-01-20");
            AssertSingleError(response, "Invalid start date.");
        }

        [Test]
        public void GivenNoEndDate_WhenCallSearchForSessions_ThenSessionSearchFailsWithInvalidEndDateError()
        {
            var useCase = new SessionSearchUseCase(null);
            var response = useCase.SearchForSessions("2015-01-13", null);
            AssertSingleError(response, "Invalid end date.");
        }

        [Test]
        public void GivenNoValidEndDate_WhenCallSearchForSessions_ThenSessionSearchFailsWithInvalidEndDateError()
        {
            var useCase = new SessionSearchUseCase(null);
            var response = useCase.SearchForSessions("2015-01-13", "xyz");
            AssertSingleError(response, "Invalid end date.");
        }

        [Test]
        public void GivenNoSessions_WhenCallSearchForSessions_ThenSessionSearchWillReturnEmptySet()
        {
            var useCase = new SessionSearchUseCase(null);
            var response = useCase.SearchForSessions("2015-01-13", "2015-01-20");

            Assert.That(response, Is.Not.Null);
            var payload = response.Data;
            Assert.That(payload, Is.Not.Null);
            Assert.That(payload, Is.InstanceOf<IEnumerable<SessionData>>());
            Assert.That(payload.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GivenSingleSessionInTimePeriod_WhenCallSearchForSessions_ThenSessionSearchWillReturnSingleSession()
        {
            var useCase = new SessionSearchUseCase(null);
            var response = useCase.SearchForSessions("2015-01-13", "2015-01-20");

            Assert.That(response, Is.Not.Null);
            var payload = response.Data;
            Assert.That(payload, Is.Not.Null);
            Assert.That(payload, Is.InstanceOf<IEnumerable<SessionData>>());
            Assert.That(payload.Count(), Is.EqualTo(0));
        }
    }
}
