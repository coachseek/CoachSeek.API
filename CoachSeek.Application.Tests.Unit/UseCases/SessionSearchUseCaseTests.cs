using System;
using System.Collections;
using System.Collections.Generic;
using CoachSeek.Application.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class SessionSearchUseCaseTests : UseCaseTests
    {
        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            ConfigureAutoMapper();
        }

        [SetUp]
        public void Setup()
        {
            SetupBusinessRepository();
        }


        [Test]
        public void GivenNoStartDate_WhenCallSearchForSessions_ThenSessionSearchFailsWithInvalidMissingStartDateError()
        {
            var criteria = GivenNoStartDate();
            var response = WhenCallSearchForSessions(criteria);
            ThenSessionSearchFailsWithMissingStartDateError(response);
        }

        [Test]
        public void GivenNoValidStartDate_WhenCallSearchForSessions_ThenSessionSearchFailsWithInvalidStartDateError()
        {
            var criteria = GivenNoValidStartDate();
            var response = WhenCallSearchForSessions(criteria);
            ThenSessionSearchFailsWithInvalidStartDateError(response);
        }

        [Test]
        public void GivenNoEndDate_WhenCallSearchForSessions_ThenSessionSearchFailsWithMissingEndDateError()
        {
            var criteria = GivenNoEndDate();
            var response = WhenCallSearchForSessions(criteria);
            ThenSessionSearchFailsWithMissingEndDateError(response);
        }

        [Test]
        public void GivenNoValidEndDate_WhenCallSearchForSessions_ThenSessionSearchFailsWithInvalidEndDateError()
        {
            var criteria = GivenNoValidEndDate();
            var response = WhenCallSearchForSessions(criteria);
            ThenSessionSearchFailsWithInvalidEndDateError(response);
        }

        [Test]
        public void GivenNoValidStartAndEndDate_WhenCallSearchForSessions_ThenSessionSearchFailsWithInvalidStartAndEndDateErrors()
        {
            var criteria = GivenNoValidStartAndEndDate();
            var response = WhenCallSearchForSessions(criteria);
            ThenSessionSearchFailsWithInvalidStartAndEndDateErrors(response);
        }

        [Test]
        public void GivenStartDateIsAfterEndDate_WhenCallSearchForSessions_ThenSessionSearchFailsWithStartDateIsAfterEndDateError()
        {
            var criteria = GivenStartDateIsAfterEndDate();
            var response = WhenCallSearchForSessions(criteria);
            ThenSessionSearchFailsWithStartDateIsAfterEndDateError(response);
        }

        [Test]
        public void GivenNoMatchingSessions_WhenCallSearchForSessions_ThenSessionSearchWillReturnEmptyList()
        {
            var criteria = GivenNoMatchingSessions();
            var response = WhenCallSearchForSessions(criteria);
            ThenSessionSearchWillReturnEmptyList(response);
        }

        [Test]
        public void GivenSingleMatchingSession_WhenCallSearchForSessions_ThenSessionSearchWillReturnSingleSession()
        {
            var criteria = GivenSingleMatchingSession();
            var response = WhenCallSearchForSessions(criteria);
            ThenSessionSearchWillReturnSingleSession(response);
        }

        [Test]
        public void GivenMultipleMatchingSessions_WhenCallSearchForSessions_ThenSessionSearchWillReturnMultipleSessions()
        {
            var criteria = GivenMultipleMatchingSessions();
            var response = WhenCallSearchForSessions(criteria);
            ThenSessionSearchWillReturnMultipleSessions(response);
        }


        private Tuple<string, string> GivenNoStartDate()
        {
            return new Tuple<string, string>(null, "2015-01-26");
        }

        private Tuple<string, string> GivenNoValidStartDate()
        {
            return new Tuple<string, string>("abc", "2015-01-26");
        }

        private Tuple<string, string> GivenNoEndDate()
        {
            return new Tuple<string, string>("2015-01-20", null);
        }

        private Tuple<string, string> GivenNoValidEndDate()
        {
            return new Tuple<string, string>("2015-01-20", "xyz");
        }

        private Tuple<string, string> GivenNoValidStartAndEndDate()
        {
            return new Tuple<string, string>("hello", "world!");
        }

        private Tuple<string, string> GivenStartDateIsAfterEndDate()
        {
            return new Tuple<string, string>("2015-01-26", "2015-01-20");
        }

        private Tuple<string, string> GivenNoMatchingSessions()
        {
            return new Tuple<string, string>("2015-01-13", "2015-01-18");
        }

        private Tuple<string, string> GivenSingleMatchingSession()
        {
            return new Tuple<string, string>("2015-01-22", "2015-01-24");
        }

        private Tuple<string, string> GivenMultipleMatchingSessions()
        {
            return new Tuple<string, string>("2015-01-21", "2015-01-25");
        }


        private object WhenCallSearchForSessions(Tuple<string, string> criteria)
        {
            var useCase = new SessionSearchUseCase(BusinessRepository);
            useCase.BusinessId = new Guid(BUSINESS_ID);

            try
            {
                return useCase.SearchForSessions(criteria.Item1, criteria.Item2);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        private void ThenSessionSearchFailsWithMissingStartDateError(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = ((ValidationException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(1));
            var error = errors[0];
            Assert.That(error.Message, Is.EqualTo("The startDate is missing."));
            Assert.That(error.Field, Is.EqualTo("startDate"));
        }

        private void ThenSessionSearchFailsWithInvalidStartDateError(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = ((ValidationException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(1));
            var error = errors[0];
            Assert.That(error.Message, Is.EqualTo("The startDate is not a valid date."));
            Assert.That(error.Field, Is.EqualTo("startDate"));
        }

        private void ThenSessionSearchFailsWithMissingEndDateError(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = ((ValidationException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(1));
            var error = errors[0];
            Assert.That(error.Message, Is.EqualTo("The endDate is missing."));
            Assert.That(error.Field, Is.EqualTo("endDate"));
        }

        private void ThenSessionSearchFailsWithInvalidEndDateError(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = ((ValidationException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(1));
            var error = errors[0];
            Assert.That(error.Message, Is.EqualTo("The endDate is not a valid date."));
            Assert.That(error.Field, Is.EqualTo("endDate"));
        }

        private void ThenSessionSearchFailsWithInvalidStartAndEndDateErrors(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = ((ValidationException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(2));
            var firstError = errors[0];
            Assert.That(firstError.Message, Is.EqualTo("The startDate is not a valid date."));
            Assert.That(firstError.Field, Is.EqualTo("startDate"));
            var secondError = errors[1];
            Assert.That(secondError.Message, Is.EqualTo("The endDate is not a valid date."));
            Assert.That(secondError.Field, Is.EqualTo("endDate"));
        }

        private void ThenSessionSearchFailsWithStartDateIsAfterEndDateError(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = ((ValidationException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(1));
            var error = errors[0];
            Assert.That(error.Message, Is.EqualTo("The startDate is after the endDate."));
            Assert.That(error.Field, Is.EqualTo("startDate"));
        }

        private void ThenSessionSearchWillReturnEmptyList(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<IList<SessionData>>());
            var sessions = (IList<SessionData>)response;
            Assert.That(sessions.Count, Is.EqualTo(0));
        }

        private void ThenSessionSearchWillReturnSingleSession(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<IList<SessionData>>());
            var sessions = (IList<SessionData>)response;
            Assert.That(sessions.Count, Is.EqualTo(1));
            var session = sessions[0];
            Assert.That(session.Id, Is.EqualTo(new Guid(SESSION_THREE)));
        }

        private void ThenSessionSearchWillReturnMultipleSessions(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<IList<SessionData>>());
            var sessions = (IList<SessionData>)response;
            Assert.That(sessions.Count, Is.EqualTo(3));

            var sessionOne = sessions[0];
            Assert.That(sessionOne.Id, Is.EqualTo(new Guid(SESSION_TWO)));
            var sessionTwo = sessions[1];
            Assert.That(sessionTwo.Id, Is.EqualTo(new Guid(SESSION_THREE)));
            var sessionThree = sessions[2];
            Assert.That(sessionThree.Id, Is.EqualTo(new Guid(SESSION_FOUR)));
        }
    }
}
