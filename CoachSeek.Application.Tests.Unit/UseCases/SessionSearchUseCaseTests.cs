using System;
using System.Collections.Generic;
using CoachSeek.Application.Contracts.Models;
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

        [Test]
        public void GivenNoValidCoachId_WhenCallSearchForSessions_ThenSessionSearchFailsWithInvalidCoachIdError()
        {
            var criteria = GivenNoValidCoachId();
            var response = WhenCallSearchForSessions(criteria);
            ThenSessionSearchFailsWithInvalidCoachIdError(response);
        }

        [Test]
        public void GivenValidCoachId_WhenCallSearchForSessions_ThenSessionSearchWillReturnSessionsForCoach()
        {
            var criteria = GivenValidCoachId();
            var response = WhenCallSearchForSessions(criteria);
            ThenSessionSearchWillReturnSessionsForCoach(response);
        }

        [Test]
        public void GivenNoValidLocationId_WhenCallSearchForSessions_ThenSessionSearchFailsWithInvalidLocationIdError()
        {
            var criteria = GivenNoValidLocationId();
            var response = WhenCallSearchForSessions(criteria);
            ThenSessionSearchFailsWithInvalidLocationIdError(response);
        }

        [Test]
        public void GivenValidLocationId_WhenCallSearchForSessions_ThenSessionSearchWillReturnSessionsForLocation()
        {
            var criteria = GivenValidLocationId();
            var response = WhenCallSearchForSessions(criteria);
            ThenSessionSearchWillReturnSessionsForLocation(response);
        }


        private Tuple<string, string, Guid?, Guid?, Guid?> GivenNoStartDate()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>(null, "2015-01-26", null, null, null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenNoValidStartDate()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("abc", "2015-01-26", null, null, null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenNoEndDate()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-20", null, null, null, null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenNoValidEndDate()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-20", "xyz", null, null, null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenNoValidStartAndEndDate()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("hello", "world!", null, null, null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenStartDateIsAfterEndDate()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-26", "2015-01-20", null, null, null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenNoMatchingSessions()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-13", "2015-01-18", null, null, null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenSingleMatchingSession()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-22", "2015-01-24", null, null, null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenMultipleMatchingSessions()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-21", "2015-01-25", null, null, null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenNoValidCoachId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-21", "2015-01-25", Guid.NewGuid(), null, null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenValidCoachId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-20", "2015-01-25", new Guid(COACH_ALBERT_ID), null, null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenNoValidLocationId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-21", "2015-01-25", null, Guid.NewGuid(), null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenValidLocationId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-21", "2015-01-27", null, new Guid(LOCATION_BROWNS_BAY_ID), null);
        }


        private object WhenCallSearchForSessions(Tuple<string, string, Guid?, Guid?, Guid?> criteria)
        {
            var businessContext = new BusinessContext(new Guid(BUSINESS_ID), "", BusinessRepository, null, null);
            var emailContext = new EmailContext(true, false, "", null);
            var context = new ApplicationContext(businessContext, emailContext, true);
            var coachGetByIdUseCase = new CoachGetByIdUseCase();
            coachGetByIdUseCase.Initialise(context);
            var locationGetByIdUseCase = new LocationGetByIdUseCase();
            locationGetByIdUseCase.Initialise(context);
            var serviceGetByIdUseCase = new ServiceGetByIdUseCase();
            serviceGetByIdUseCase.Initialise(context);

            var useCase = new SessionSearchUseCase(coachGetByIdUseCase, locationGetByIdUseCase, serviceGetByIdUseCase);
            useCase.Initialise(context);

            try
            {
                return useCase.SearchForSessions(criteria.Item1, criteria.Item2, criteria.Item3, criteria.Item4);
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
            Assert.That(response, Is.InstanceOf<SessionSearchData>());
            var searchData = (SessionSearchData) response;
            var sessions = searchData.Sessions;
            Assert.That(sessions.Count, Is.EqualTo(0));
            var courses = searchData.Courses;
            Assert.That(courses.Count, Is.EqualTo(0));
        }

        private void ThenSessionSearchWillReturnSingleSession(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<SessionSearchData>());
            var searchData = (SessionSearchData)response;
            var sessions = searchData.Sessions;
            Assert.That(sessions.Count, Is.EqualTo(1));
            var session = sessions[0];
            Assert.That(session.Id, Is.EqualTo(new Guid(SESSION_THREE)));
            var courses = searchData.Courses;
            Assert.That(courses.Count, Is.EqualTo(0));
        }

        private void ThenSessionSearchWillReturnMultipleSessions(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<IList<SingleSessionData>>());
            var sessions = (IList<SingleSessionData>)response;
            Assert.That(sessions.Count, Is.EqualTo(3));

            var sessionOne = sessions[0];
            Assert.That(sessionOne.Id, Is.EqualTo(new Guid(SESSION_TWO)));
            var sessionTwo = sessions[1];
            Assert.That(sessionTwo.Id, Is.EqualTo(new Guid(SESSION_THREE)));
            var sessionThree = sessions[2];
            Assert.That(sessionThree.Id, Is.EqualTo(new Guid(SESSION_FOUR)));
        }

        private void ThenSessionSearchFailsWithInvalidCoachIdError(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = ((ValidationException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(1));
            var error = errors[0];
            Assert.That(error.Message, Is.EqualTo("Not a valid coachId."));
            Assert.That(error.Field, Is.EqualTo("coachId"));
        }

        private void ThenSessionSearchWillReturnSessionsForCoach(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<IList<SingleSessionData>>());
            var sessions = (IList<SingleSessionData>)response;
            Assert.That(sessions.Count, Is.EqualTo(2));

            var sessionOne = sessions[0];
            Assert.That(sessionOne.Id, Is.EqualTo(new Guid(SESSION_ONE)));
            var sessionTwo = sessions[1];
            Assert.That(sessionTwo.Id, Is.EqualTo(new Guid(SESSION_THREE)));
        }

        private void ThenSessionSearchFailsWithInvalidLocationIdError(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = ((ValidationException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(1));
            var error = errors[0];
            Assert.That(error.Message, Is.EqualTo("Not a valid locationId."));
            Assert.That(error.Field, Is.EqualTo("locationId"));
        }

        private void ThenSessionSearchWillReturnSessionsForLocation(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<IList<SingleSessionData>>());
            var sessions = (IList<SingleSessionData>)response;
            Assert.That(sessions.Count, Is.EqualTo(2));

            var sessionOne = sessions[0];
            Assert.That(sessionOne.Id, Is.EqualTo(new Guid(SESSION_TWO)));
            var sessionTwo = sessions[1];
            Assert.That(sessionTwo.Id, Is.EqualTo(new Guid(SESSION_FIVE)));
        }
    }
}
