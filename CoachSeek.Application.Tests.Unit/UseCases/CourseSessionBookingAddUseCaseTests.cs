using System;
using System.Collections.Generic;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.UseCases;
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

            UseCase = new CourseSessionBookingAddUseCase();
            UseCase.Initialise(CreateApplicationContext());
            UseCase.Course = BusinessRepository.GetCourse(new Guid(BUSINESS_ID), new Guid(COURSE_ONE));
        }

        [Test]
        public void GivenASessionDoesNotBelongToTheCourse_WhenTryAddBooking_ThenReturnSessionNotInCourseError()
        {
            var command = GivenASessionDoesNotBelongToTheCourse();
            var response = WhenTryAddBooking(command);
            ThenReturnSessionNotInCourseError(response);
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
            ThenReturnInvalidCustomerError(response);
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


        private void ThenReturnSessionNotInCourseError(IResponse response)
        {
            AssertSingleError(response, "One or more of the sessions is not in the course.", "booking.sessions");
        }

        private void ThenReturnDuplicateSessionInCourseError(IResponse response)
        {
            AssertSingleError(response, "Some sessions are duplicates.", "booking.sessions");
        }

        private void ThenReturnInvalidCustomerError(IResponse response)
        {
            AssertSingleError(response, "This customer does not exist.", "booking.customer.id");
        }
    }
}
