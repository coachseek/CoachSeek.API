using System;
using System.Collections.Generic;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.UseCases;
using CoachSeek.Domain.Commands;
using NUnit.Framework;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class StandaloneSessionBookingAddUseCaseTests : UseCaseTests
    {
        private StandaloneSessionBookingAddUseCase UseCase { get; set; }


        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            ConfigureAutoMapper();
        }

        [SetUp]
        public void Setup()
        {
            SetupBusinessRepository();

            UseCase = new StandaloneSessionBookingAddUseCase();
            UseCase.Initialise(CreateApplicationContext());
        }

        [Test]
        public void GivenMultipleSessions_WhenTryAddBooking_ThenReturnMultipleSessionsError()
        {
            var command = GivenMultipleSessions();
            var response = WhenTryAddBooking(command);
            ThenReturnMultipleSessionsError(response);
        }

        [Test]
        public void GivenNonExistentCustomer_WhenTryAddBooking_ThenReturnInvalidCustomerError()
        {
            var command = GivenNonExistentCustomer();
            var response = WhenTryAddBooking(command);
            ThenReturnInvalidCustomerError(response);
        }

        [Test]
        public void GivenMultipleErrorsInCommand_WhenTryAddBooking_ThenReturnMultipleErrors()
        {
            var command = GivenMultipleErrorsInCommand();
            var response = WhenTryAddBooking(command);
            ThenReturnMultipleErrors(response);
        }


        private BookingAddCommand GivenMultipleSessions()
        {
            return new BookingAddCommand
            {
                Sessions = new List<SessionKeyCommand>
                {
                    new SessionKeyCommand { Id = new Guid(SESSION_TWO) },
                    new SessionKeyCommand { Id = new Guid(COURSE_ONE_SESSION_ONE) }
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
                    new SessionKeyCommand { Id = new Guid(SESSION_TWO) }
                },
                Customer = new CustomerKeyCommand { Id = Guid.NewGuid() }
            };
        }

        private BookingAddCommand GivenMultipleErrorsInCommand()
        {
            return new BookingAddCommand
            {
                Sessions = new List<SessionKeyCommand>
                {
                    new SessionKeyCommand { Id = new Guid(SESSION_TWO) },
                    new SessionKeyCommand { Id = new Guid(COURSE_ONE_SESSION_ONE) }
                },
                Customer = new CustomerKeyCommand { Id = Guid.NewGuid() }
            };
        }


        private Response WhenTryAddBooking(BookingAddCommand command)
        {
            return UseCase.AddBooking(command);
        }


        private void ThenReturnMultipleSessionsError(Response response)
        {
            AssertSingleError(response, "Standalone sessions must be booked one at a time.", "booking.sessions");
        }

        private void ThenReturnInvalidCustomerError(Response response)
        {
            AssertSingleError(response, "This customer does not exist.", "booking.customer.id");
        }

        private void ThenReturnMultipleErrors(Response response)
        {
            AssertMultipleErrors(response, new[,] { { "Standalone sessions must be booked one at a time.", "booking.sessions" },
                                                    { "This customer does not exist.", "booking.customer.id" } });
        }
    }
}
