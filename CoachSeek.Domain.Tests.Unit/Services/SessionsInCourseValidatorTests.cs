using System;
using System.Collections.Generic;
using CoachSeek.Common;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Services;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Services
{
    [TestFixture]
    public class SessionsInCourseValidatorTests
    {
        private const string COURSE_ID = "CCCF1760-D46E-4308-A88D-10EC075A73B7";
        private const string SESSION_ONE_ID = "1AA25A07-BC8E-4199-8EBD-C1E7212F3426";
        private const string SESSION_TWO_ID = "2BB4A557-6D49-4D7B-96B9-146C360373D6";
        private const string SESSION_THREE_ID = "3CC7C97C-8E99-4931-BE3D-92831CBDB792";

        private RepeatedSessionData SetupCourse()
        {
            return new RepeatedSessionData
            {
                Id = new Guid(COURSE_ID),
                Sessions = new []
                {
                    new SingleSessionData
                    {
                        Id = new Guid(SESSION_ONE_ID),
                        ParentId = new Guid(COURSE_ID)
                    }, 
                    new SingleSessionData
                    {
                        Id = new Guid(SESSION_TWO_ID),
                        ParentId = new Guid(COURSE_ID)
                    }, 
                    new SingleSessionData
                    {
                        Id = new Guid(SESSION_THREE_ID),
                        ParentId = new Guid(COURSE_ID)
                    }
                }
            };
        }


        [Test]
        public void GivenNoSessions_WhenTryValidate_ThenValidationSucceeds()
        {
            var course = SetupCourse();

            var sessionsToCheck = GivenNoSessions();
            var response = WhenTryValidate(sessionsToCheck, course);
            ThenValidationSucceeds(response);
        }

        [Test]
        public void GivenSingleSessionIsInCourse_WhenTryValidate_ThenValidationSucceeds()
        {
            var course = SetupCourse();

            var sessionsToCheck = GivenSingleSessionIsInCourse();
            var response = WhenTryValidate(sessionsToCheck, course);
            ThenValidationSucceeds(response);
        }

        [Test]
        public void GivenSeveralSessionsInCourse_WhenTryValidate_ThenValidationSucceeds()
        {
            var course = SetupCourse();

            var sessionsToCheck = GivenSeveralSessionsInCourse();
            var response = WhenTryValidate(sessionsToCheck, course);
            ThenValidationSucceeds(response);
        }

        [Test]
        public void GivenAllSessionsInCourse_WhenTryValidate_ThenValidationSucceeds()
        {
            var course = SetupCourse();

            var sessionsToCheck = GivenAllSessionsInCourse();
            var response = WhenTryValidate(sessionsToCheck, course);
            ThenValidationSucceeds(response);
        }

        [Test]
        public void GivenSingleSessionNotInCourse_WhenTryValidate_ThenValidationFailsWithSessionNotInCourseError()
        {
            var course = SetupCourse();

            var sessionIdNotInCourse = Guid.NewGuid();
            var sessionsToCheck = GivenSingleSessionNotInCourse(sessionIdNotInCourse);
            var response = WhenTryValidate(sessionsToCheck, course);
            ThenValidationFailsWithSessionNotInCourseError(response, sessionIdNotInCourse);
        }

        [Test]
        public void GivenSeveralSessionsNotInCourse_WhenTryValidate_ThenValidationFailsWithSessionNotInCourseError()
        {
            var course = SetupCourse();

            var sessionId = Guid.NewGuid();
            var session2Id = Guid.NewGuid();
            var sessionsToCheck = GivenSeveralSessionsNotInCourse(sessionId, session2Id);
            var response = WhenTryValidate(sessionsToCheck, course);
            ThenValidationFailsWithSeveralSessionsNotInCourseError(response, sessionId, session2Id);
        }

        [Test]
        public void GivenDuplicateSessionInCourse_WhenTryValidate_ThenValidationFailsWithDuplicateSessionError()
        {
            var course = SetupCourse();

            var sessionsToCheck = GivenDuplicateSessionInCourse(SESSION_TWO_ID);
            var response = WhenTryValidate(sessionsToCheck, course);
            ThenValidationFailsWithDuplicateSessionError(response, SESSION_TWO_ID);
        }

        [Test]
        public void GivenSeveralErrors_WhenTryValidate_ThenValidationFailsWithSeveralErrors()
        {
            var course = SetupCourse();

            var sessionId = Guid.NewGuid();
            var session2Id = Guid.NewGuid();
            var sessionsToCheck = GivenSeveralErrors(sessionId, session2Id);
            var response = WhenTryValidate(sessionsToCheck, course);
            ThenValidationFailsWithSeveralErrors(response, sessionId, session2Id);
        }


        private IEnumerable<SessionKeyCommand> GivenNoSessions()
        {
            return new List<SessionKeyCommand>();
        }

        private IEnumerable<SessionKeyCommand> GivenSingleSessionIsInCourse()
        {
            return new List<SessionKeyCommand>
            {
                new SessionKeyCommand(new Guid(SESSION_TWO_ID))
            };
        }

        private IEnumerable<SessionKeyCommand> GivenSeveralSessionsInCourse()
        {
            return new List<SessionKeyCommand>
            {
                new SessionKeyCommand(new Guid(SESSION_TWO_ID)),
                new SessionKeyCommand(new Guid(SESSION_THREE_ID))
            };
        }

        private IEnumerable<SessionKeyCommand> GivenAllSessionsInCourse()
        {
            return new List<SessionKeyCommand>
            {
                new SessionKeyCommand(new Guid(SESSION_ONE_ID)),
                new SessionKeyCommand(new Guid(SESSION_TWO_ID)),
                new SessionKeyCommand(new Guid(SESSION_THREE_ID))
            };
        }

        private IEnumerable<SessionKeyCommand> GivenSingleSessionNotInCourse(Guid sessionId)
        {
            return new List<SessionKeyCommand>
            {
                new SessionKeyCommand(sessionId)
            };
        }

        private IEnumerable<SessionKeyCommand> GivenSeveralSessionsNotInCourse(Guid sessionId, Guid session2Id)
        {
            return new List<SessionKeyCommand>
            {
                new SessionKeyCommand(sessionId),
                new SessionKeyCommand(new Guid(SESSION_THREE_ID)),
                new SessionKeyCommand(session2Id)
            };
        }

        private IEnumerable<SessionKeyCommand> GivenDuplicateSessionInCourse(string duplicateSessionId)
        {
            return new List<SessionKeyCommand>
            {
                new SessionKeyCommand(new Guid(duplicateSessionId)),
                new SessionKeyCommand(new Guid(SESSION_ONE_ID)),
                new SessionKeyCommand(new Guid(duplicateSessionId))
            };
        }

        private IEnumerable<SessionKeyCommand> GivenSeveralErrors(Guid randomSessionId, Guid randomSession2Id)
        {
            return new List<SessionKeyCommand>
            {
                new SessionKeyCommand(randomSessionId),
                new SessionKeyCommand(new Guid(SESSION_ONE_ID)),
                new SessionKeyCommand(new Guid(SESSION_TWO_ID)),
                new SessionKeyCommand(randomSession2Id),
                new SessionKeyCommand(new Guid(SESSION_THREE_ID)),
                new SessionKeyCommand(new Guid(SESSION_ONE_ID)),
            };
        }


        private object WhenTryValidate(IEnumerable<SessionKeyCommand> sessionsToCheck, RepeatedSessionData course)
        {
            try
            {
                SessionsInCourseValidator.Validate(sessionsToCheck, course);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private void ThenValidationSucceeds(object response)
        {
            Assert.That(response, Is.Null);
        }

        private void ThenValidationFailsWithSessionNotInCourseError(object response, Guid sessionId)
        {
            Assert.That(response, Is.InstanceOf<ValidationException>());

            var errors = ((ValidationException) response).Errors;
            Assert.That(errors.Count, Is.EqualTo(1));

            AssertSessionNotInCourseError(errors[0], sessionId);
        }

        private void ThenValidationFailsWithSeveralSessionsNotInCourseError(object response, Guid sessionId, Guid session2Id)
        {
            Assert.That(response, Is.InstanceOf<ValidationException>());

            var errors = ((ValidationException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(2));

            AssertSessionNotInCourseError(errors[0], sessionId);
            AssertSessionNotInCourseError(errors[1], session2Id);
        }

        private void ThenValidationFailsWithDuplicateSessionError(object response, string duplicateSessionId)
        {
            Assert.That(response, Is.InstanceOf<ValidationException>());

            var errors = ((ValidationException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(1));

            AssertDuplicateSessionError(errors[0]);
        }

        private void ThenValidationFailsWithSeveralErrors(object response, Guid randomSessionId, Guid randomSession2Id)
        {
            Assert.That(response, Is.InstanceOf<ValidationException>());

            var errors = ((ValidationException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(3));

            AssertSessionNotInCourseError(errors[0], randomSessionId);
            AssertSessionNotInCourseError(errors[1], randomSession2Id);
            AssertDuplicateSessionError(errors[2]);
        }

        private void AssertSessionNotInCourseError(Error error, Guid sessionId)
        {
            Assert.That(error.Code, Is.EqualTo(ErrorCodes.SessionNotInCourse));
            Assert.That(error.Data, Is.EqualTo(string.Format("Session: '{0}', Course: '{1}'",
                                                             sessionId,
                                                             new Guid(COURSE_ID))));
            Assert.That(error.Message, Is.EqualTo("Session is not in course."));
        }

        private void AssertDuplicateSessionError(Error error)
        {
            Assert.That(error.Code, Is.EqualTo(ErrorCodes.SessionDuplicate));
            Assert.That(error.Data, Is.Null);
            Assert.That(error.Message, Is.EqualTo("Duplicate session."));
        }
    }
}
