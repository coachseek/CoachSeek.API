using System;
using System.Collections.Generic;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Tests.Unit.Fakes;
using CoachSeek.Application.UseCases;
using CoachSeek.Domain.Commands;
using NUnit.Framework;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class BookingAddMasterUseCaseTests : UseCaseTests
    {
        private BookingAddMasterUseCase UseCase { get; set; }

        private StubStandaloneSessionBookingAddUseCase StandaloneSessionUseCase
        {
            get { return (StubStandaloneSessionBookingAddUseCase)UseCase.StandaloneSessionBookingAddUseCase; }
        }

        private StubCourseSessionBookingAddUseCase CourseSessionUseCase
        {
            get { return (StubCourseSessionBookingAddUseCase)UseCase.CourseSessionBookingAddUseCase; }
        }

        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            ConfigureAutoMapper();
        }

        [SetUp]
        public void Setup()
        {
            SetupBusinessRepository();

            UseCase = new BookingAddMasterUseCase(new StubStandaloneSessionBookingAddUseCase(), 
                                                  new StubStandaloneSessionOnlineBookingAddUseCase(),
                                                  new StubCourseSessionBookingAddUseCase(),
                                                  new StubCourseSessionOnlineBookingAddUseCase());
            UseCase.Initialise(CreateApplicationContext());
        }


        [Test]
        public void GivenNoSession_WhenTryAddBooking_ThenReturnMissingSessionError()
        {
            var command = GivenNoSession();
            var response = WhenTryAddBooking(command);
            ThenReturnMissingSessionError(response);
        }

        [Test]
        public void GivenSingleSessionDoesNotExist_WhenTryAddBooking_ThenReturnInvalidSessionError()
        {
            var command = GivenSingleSessionDoesNotExist();
            var response = WhenTryAddBooking(command);
            ThenReturnInvalidSessionError(response);
        }

        [Test]
        public void GivenSingleStandaloneSession_WhenTryAddBooking_ThenInvokeAddStandaloneSessionBookingUseCase()
        {
            var command = GivenSingleStandaloneSession();
            var response = WhenTryAddBooking(command);
            ThenInvokeAddStandaloneSessionBookingUseCase(response);
        }

        [Test]
        public void GivenFirstSessionIsStandalone_WhenTryAddBooking_ThenInvokeAddStandaloneSessionBookingUseCase()
        {
            var command = GivenFirstSessionIsStandalone();
            var response = WhenTryAddBooking(command);
            ThenInvokeAddStandaloneSessionBookingUseCase(response);
        }

        [Test]
        public void GivenSingleCourseSession_WhenTryAddBooking_ThenInvokeAddCourseSessionBookingUseCase()
        {
            var command = GivenSingleCourseSession();
            var response = WhenTryAddBooking(command);
            ThenInvokeAddCourseSessionBookingUseCase(response);
        }

        [Test]
        public void GivenSeveralCourseSessions_WhenTryAddBooking_ThenInvokeAddCourseSessionBookingUseCase()
        {
            var command = GivenSeveralCourseSessions();
            var response = WhenTryAddBooking(command);
            ThenInvokeAddCourseSessionBookingUseCase(response);
        }

        [Test]
        public void GivenAllCourseSessions_WhenTryAddBooking_ThenInvokeAddCourseSessionBookingUseCase()
        {
            var command = GivenAllCourseSessions();
            var response = WhenTryAddBooking(command);
            ThenInvokeAddCourseSessionBookingUseCase(response);
        }


        private BookingAddCommand GivenNoSession()
        {
            return new BookingAddCommand
            {
                Sessions = new List<SessionKeyCommand>(),
                Customer = new CustomerKeyCommand {Id = new Guid(CUSTOMER_FRED_ID)}
            };
        }

        private BookingAddCommand GivenSingleSessionDoesNotExist()
        {
            return new BookingAddCommand
            {
                Sessions = new List<SessionKeyCommand>
                {
                    new SessionKeyCommand { Id = Guid.NewGuid() }
                },
                Customer = new CustomerKeyCommand { Id = new Guid() },
            };
        }

        private BookingAddCommand GivenFirstSessionIsStandalone()
        {
            return new BookingAddCommand
            {
                Sessions = new List<SessionKeyCommand>
                {
                    new SessionKeyCommand { Id = new Guid(SESSION_TWO) },
                    new SessionKeyCommand { Id = new Guid(COURSE_ONE_SESSION_ONE) }
                },
                Customer = new CustomerKeyCommand { Id = new Guid() },
            };
        }

        private BookingAddCommand GivenSingleStandaloneSession()
        {
            return new BookingAddCommand
            {
                Sessions = new List<SessionKeyCommand>
                {
                    new SessionKeyCommand { Id = new Guid(SESSION_TWO) }
                },
                Customer = new CustomerKeyCommand { Id = new Guid() },
            };
        }

        private BookingAddCommand GivenSingleCourseSession()
        {
            return new BookingAddCommand
            {
                Sessions = new List<SessionKeyCommand>
                {
                    new SessionKeyCommand { Id = new Guid(COURSE_ONE_SESSION_ONE) }
                },
                Customer = new CustomerKeyCommand { Id = new Guid() },
            };
        }

        private BookingAddCommand GivenSeveralCourseSessions()
        {
            return new BookingAddCommand
            {
                Sessions = new List<SessionKeyCommand>
                {
                    new SessionKeyCommand { Id = new Guid(COURSE_ONE_SESSION_ONE) },
                    new SessionKeyCommand { Id = new Guid(COURSE_ONE_SESSION_TWO) }
                },
                Customer = new CustomerKeyCommand { Id = new Guid() },
            };
        }

        private BookingAddCommand GivenAllCourseSessions()
        {
            return new BookingAddCommand
            {
                Sessions = new List<SessionKeyCommand>
                {
                    new SessionKeyCommand { Id = new Guid(COURSE_ONE_SESSION_ONE) },
                    new SessionKeyCommand { Id = new Guid(COURSE_ONE_SESSION_TWO) },
                    new SessionKeyCommand { Id = new Guid(COURSE_ONE_SESSION_THREE) }
                },
                Customer = new CustomerKeyCommand { Id = new Guid() },
            };
        }


        private Response WhenTryAddBooking(BookingAddCommand command)
        {
            return UseCase.AddBooking(command);
        }


        private void ThenReturnMissingSessionError(Response response)
        {
            AssertSingleError(response, "A booking must have at least one session.");
        }

        private void ThenReturnInvalidSessionError(Response response)
        {
            AssertSingleError(response, "This session does not exist.");
        }

        private void ThenInvokeAddStandaloneSessionBookingUseCase(Response response)
        {
            Assert.That(StandaloneSessionUseCase.WasSessionSet, Is.True);
            Assert.That(StandaloneSessionUseCase.WasInitialiseCalled, Is.True);
            Assert.That(StandaloneSessionUseCase.WasAddBookingCalled, Is.True);

            Assert.That(CourseSessionUseCase.WasCourseSet, Is.False);
            Assert.That(CourseSessionUseCase.WasInitialiseCalled, Is.False);
            Assert.That(CourseSessionUseCase.WasAddBookingCalled, Is.False);
        }

        private void ThenInvokeAddCourseSessionBookingUseCase(Response response)
        {
            Assert.That(StandaloneSessionUseCase.WasSessionSet, Is.False);
            Assert.That(StandaloneSessionUseCase.WasInitialiseCalled, Is.False);
            Assert.That(StandaloneSessionUseCase.WasAddBookingCalled, Is.False);

            Assert.That(CourseSessionUseCase.WasCourseSet, Is.True);
            Assert.That(CourseSessionUseCase.WasInitialiseCalled, Is.True);
            Assert.That(CourseSessionUseCase.WasAddBookingCalled, Is.True);
        }
    }
}
