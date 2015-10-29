using System;
using System.Collections.Generic;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.UseCases;
using CoachSeek.Common;
using CoachSeek.Domain.Commands;
using NUnit.Framework;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class CourseSessionBookingAddUseCaseTests : UseCaseTests
    {
        private CourseSessionBookingAddUseCase UseCase { get; set; }


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

            UseCase = new CourseSessionBookingAddUseCase();
            UseCase.Initialise(CreateApplicationContext());
            UseCase.Course = BusinessRepository.GetCourse(new Guid(BUSINESS_ID), new Guid(COURSE_ONE));
        }

        [Test]
        public void GivenASessionDoesNotBelongToTheCourse_WhenTryAddBooking_ThenReturnSessionNotInCourseError()
        {
            var command = GivenASessionDoesNotBelongToTheCourse();
            var response = WhenTryAddBooking(command);
            ThenReturnSessionNotInCourseError(response, command.Sessions[1].Id, new Guid(COURSE_ONE));
        }

        [Test]
        public void GivenDuplicateSessionInCourse_WhenTryAddBooking_ThenReturnDuplicateSessionInCourseError()
        {
            var command = GivenDuplicateSessionInCourse();
            var response = WhenTryAddBooking(command);
            ThenReturnDuplicateSessionInCourseError(response);
        }

        [Test]
        public void GivenNonExistentCustomer_WhenTryAddBooking_ThenReturnInvalidCustomerError()
        {
            var command = GivenNonExistentCustomer();
            var response = WhenTryAddBooking(command);
            ThenReturnInvalidCustomerError(response, command.Customer.Id);
        }


        private BookingAddCommand GivenASessionDoesNotBelongToTheCourse()
        {
            return new BookingAddCommand
            {
                Sessions = new List<SessionKeyCommand>
                {
                    new SessionKeyCommand { Id = new Guid(COURSE_ONE_SESSION_ONE) },
                    new SessionKeyCommand { Id = Guid.NewGuid() },
                },
                Customer = new CustomerKeyCommand { Id = new Guid(CUSTOMER_FRED_ID) }
            };
        }

        private BookingAddCommand GivenDuplicateSessionInCourse()
        {
            return new BookingAddCommand
            {
                Sessions = new List<SessionKeyCommand>
                {
                    new SessionKeyCommand { Id = new Guid(COURSE_ONE_SESSION_ONE) },
                    new SessionKeyCommand { Id = new Guid(COURSE_ONE_SESSION_THREE) },
                    new SessionKeyCommand { Id = new Guid(COURSE_ONE_SESSION_THREE) }
                },
                Customer = new CustomerKeyCommand { Id = new Guid(CUSTOMER_FRED_ID) }
            };
        }

        private BookingAddCommand GivenNonExistentCustomer()
        {
            return new BookingAddCommand
            {
                Sessions = new List<SessionKeyCommand>
                {
                    new SessionKeyCommand { Id = new Guid(COURSE_ONE_SESSION_ONE) },
                    new SessionKeyCommand { Id = new Guid(COURSE_ONE_SESSION_THREE) }
                },
                Customer = new CustomerKeyCommand { Id = Guid.NewGuid() }
            };
        }


        private IResponse WhenTryAddBooking(BookingAddCommand command)
        {
            return UseCase.AddBooking(command);
        }


        private void ThenReturnSessionNotInCourseError(IResponse response, Guid sessionId, Guid courseId)
        {
            AssertSingleError(response, 
                              ErrorCodes.SessionNotInCourse,
                              "Session is not in course.",
                              string.Format("Session: '{0}', Course: '{1}'", sessionId, courseId));
        }

        private void ThenReturnDuplicateSessionInCourseError(IResponse response)
        {
            AssertSingleError(response, 
                              ErrorCodes.SessionDuplicate, 
                              "Duplicate session.");
        }

        private void ThenReturnInvalidCustomerError(IResponse response, Guid customerId)
        {
            AssertSingleError(response, ErrorCodes.CustomerInvalid, "This customer does not exist.", customerId.ToString());
        }
    }
}
