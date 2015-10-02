using System;
using CoachSeek.Application.UseCases;
using CoachSeek.Common;
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
            SetupUserRepository();
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
            ThenSessionSearchFailsWithInvalidCoachIdError(response, criteria.Item3.Value);
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
            ThenSessionSearchFailsWithInvalidLocationIdError(response, criteria.Item4.Value);
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
            var context = CreateApplicationContext();
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
                var task = useCase.SearchForSessionsAsync(criteria.Item1, criteria.Item2, criteria.Item3, criteria.Item4);
                return task.Result;
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
            Assert.That(error.Code, Is.EqualTo(ErrorCodes.StartDateRequired));
            Assert.That(error.Message, Is.EqualTo("The StartDate field is required."));
            Assert.That(error.Data, Is.Null);
        }

        private void ThenSessionSearchFailsWithInvalidStartDateError(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = ((ValidationException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(1));

            var error = errors[0];
            Assert.That(error.Code, Is.EqualTo(ErrorCodes.StartDateInvalid));
            Assert.That(error.Message, Is.EqualTo("'abc' is not a valid start date."));
            Assert.That(error.Data, Is.EqualTo("abc"));
        }

        private void ThenSessionSearchFailsWithMissingEndDateError(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = ((ValidationException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(1));

            var error = errors[0];
            Assert.That(error.Code, Is.EqualTo(ErrorCodes.EndDateRequired));
            Assert.That(error.Message, Is.EqualTo("The EndDate field is required."));
            Assert.That(error.Data, Is.Null);
        }

        private void ThenSessionSearchFailsWithInvalidEndDateError(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = ((ValidationException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(1));

            var error = errors[0];
            Assert.That(error.Code, Is.EqualTo(ErrorCodes.EndDateInvalid));
            Assert.That(error.Message, Is.EqualTo("'xyz' is not a valid end date."));
            Assert.That(error.Data, Is.EqualTo("xyz"));
        }

        private void ThenSessionSearchFailsWithInvalidStartAndEndDateErrors(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = ((ValidationException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(2));

            var firstError = errors[0];
            Assert.That(firstError.Code, Is.EqualTo(ErrorCodes.StartDateInvalid));
            Assert.That(firstError.Message, Is.EqualTo("'hello' is not a valid start date."));
            Assert.That(firstError.Data, Is.EqualTo("hello"));

            var secondError = errors[1];
            Assert.That(secondError.Code, Is.EqualTo(ErrorCodes.EndDateInvalid));
            Assert.That(secondError.Message, Is.EqualTo("'world!' is not a valid end date."));
            Assert.That(secondError.Data, Is.EqualTo("world!"));
        }

        private void ThenSessionSearchFailsWithStartDateIsAfterEndDateError(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<CoachseekException>());
            var errors = ((CoachseekException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(1));

            var error = errors[0];
            Assert.That(error.Code, Is.EqualTo(ErrorCodes.StartDateAfterEndDate));
            Assert.That(error.Message, Is.EqualTo("Start date '2015-01-26' is after end date '2015-01-20'"));
            Assert.That(error.Data, Is.EqualTo("Start date: '2015-01-26', End date: '2015-01-20'"));
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
            Assert.That(courses.Count, Is.EqualTo(1));
        }

        private void ThenSessionSearchWillReturnMultipleSessions(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<SessionSearchData>());
            var searchData = (SessionSearchData)response;

            var sessions = searchData.Sessions;
            Assert.That(sessions.Count, Is.EqualTo(3));

            var sessionOne = sessions[0];
            Assert.That(sessionOne.Id, Is.EqualTo(new Guid(SESSION_TWO)));
            var sessionTwo = sessions[1];
            Assert.That(sessionTwo.Id, Is.EqualTo(new Guid(SESSION_THREE)));
            var sessionThree = sessions[2];
            Assert.That(sessionThree.Id, Is.EqualTo(new Guid(SESSION_FOUR)));

            var courses = searchData.Courses;
            Assert.That(courses.Count, Is.EqualTo(1));
        }

        private void ThenSessionSearchFailsWithInvalidCoachIdError(object response, Guid invalidCoachId)
        {
            AssertInvalidCoach(response, invalidCoachId);
        }

        private void ThenSessionSearchWillReturnSessionsForCoach(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<SessionSearchData>());
            var searchResult = (SessionSearchData)response;

            var sessions = searchResult.Sessions;
            Assert.That(sessions.Count, Is.EqualTo(2));

            var sessionOne = sessions[0];
            Assert.That(sessionOne.Id, Is.EqualTo(new Guid(SESSION_ONE)));
            var sessionTwo = sessions[1];
            Assert.That(sessionTwo.Id, Is.EqualTo(new Guid(SESSION_THREE)));

            var courses = searchResult.Courses;
            Assert.That(courses.Count, Is.EqualTo(0));
        }

        private void ThenSessionSearchFailsWithInvalidLocationIdError(object response, Guid invalidLocationId)
        {
            AssertInvalidLocation(response, invalidLocationId);
        }

        private void ThenSessionSearchWillReturnSessionsForLocation(object response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<SessionSearchData>());
            var searchResult = (SessionSearchData)response;

            var sessions = searchResult.Sessions; 
            Assert.That(sessions.Count, Is.EqualTo(2));

            var sessionOne = sessions[0];
            Assert.That(sessionOne.Id, Is.EqualTo(new Guid(SESSION_TWO)));
            var sessionTwo = sessions[1];
            Assert.That(sessionTwo.Id, Is.EqualTo(new Guid(SESSION_FIVE)));
        }
    }
}
