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
            ThenReturnInvalidCustomerError(response, command.Customer.Id);
        }

        [Test]
        public void GivenMultipleErrorsInCommand_WhenTryAddBooking_ThenReturnMultipleErrors()
        {
            var command = GivenMultipleErrorsInCommand();
            var response = WhenTryAddBooking(command);
            ThenReturnMultipleErrors(response, command);
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


        private IResponse WhenTryAddBooking(BookingAddCommand command)
        {
            return UseCase.AddBooking(command);
        }


        private void ThenReturnMultipleSessionsError(IResponse response)
        {
            AssertSingleError(response, 
                              ErrorCodes.StandaloneSessionsMustBeBookedOneAtATime, 
                              "Standalone sessions must be booked one at a time.", 
                              null);
        }

        private void ThenReturnInvalidCustomerError(IResponse response, Guid customerId)
        {
            AssertSingleError(response, ErrorCodes.CustomerInvalid, "This customer does not exist.", customerId.ToString());
        }

        private void ThenReturnMultipleErrors(IResponse response, BookingAddCommand command)
        {
            AssertMultipleErrors(response, new[,] { { ErrorCodes.StandaloneSessionsMustBeBookedOneAtATime, "Standalone sessions must be booked one at a time.", null, null },
                                                    { ErrorCodes.CustomerInvalid, "This customer does not exist.", command.Customer.Id.ToString(), null } });
        }
    }
}
