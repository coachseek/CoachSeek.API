using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.UseCases;
using CoachSeek.Common;
using CoachSeek.Domain.Commands;
using NUnit.Framework;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class BookingSetPaymentStatusUseCaseTests : UseCaseTests
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
        public void GivenNonExistentBookingId_WhenTrySetPaymentStatus_ThenReturnNotFound()
        {
            var command = GivenNonExistentBookingId();
            var response = WhenTrySetPaymentStatus(command);
            ThenReturnNotFound(response);
        }

        [Test]
        public void GivenNonExistentPaymentStatus_WhenTrySetPaymentStatus_ThenReturnInvalidPaymentStatusError()
        {
            var command = GivenNonExistentPaymentStatus();
            var response = WhenTrySetPaymentStatus(command);
            ThenReturnInvalidPaymentStatusError(response);
        }

        [Test]
        public void GivenValidBookingSetPaymentStatusCommand_WhenTrySetPaymentStatus_ThenUpdateBookingPaymentStatus()
        {
            var command = GivenValidBookingSetPaymentStatusCommand();
            var response = WhenTrySetPaymentStatus(command);
            ThenUpdateBookingPaymentStatus(response);
        }


        private BookingSetPaymentStatusCommand GivenNonExistentBookingId()
        {
            return new BookingSetPaymentStatusCommand
            {
                BookingId = Guid.NewGuid(),
                PaymentStatus = Constants.PAYMENT_STATUS_PENDING_PAYMENT
            };
        }

        private BookingSetPaymentStatusCommand GivenNonExistentPaymentStatus()
        {
            return new BookingSetPaymentStatusCommand
            {
                BookingId = new Guid(BOOKING_FRED_SESSION_TWO_ID),
                PaymentStatus = "hello world!"
            };
        }

        private BookingSetPaymentStatusCommand GivenValidBookingSetPaymentStatusCommand()
        {
            return new BookingSetPaymentStatusCommand
            {
                BookingId = new Guid(BOOKING_FRED_SESSION_TWO_ID),
                PaymentStatus = Constants.PAYMENT_STATUS_PAID
            };
        }

        
        private object WhenTrySetPaymentStatus(BookingSetPaymentStatusCommand command)
        {
            var useCase = new BookingSetPaymentStatusUseCase();
            var business = new BusinessDetails(new Guid(BUSINESS_ID), "", "");
            var currency = new CurrencyDetails("NZD", "$");
            var businessContext = new BusinessContext(business, currency, null, BusinessRepository, SupportedCurrencyRepository, null);
            var emailContext = new EmailContext(true, false, "", null);
            var context = new ApplicationContext(businessContext, emailContext, true);
            useCase.Initialise(context);

            try
            {
                return useCase.SetPaymentStatus(command);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        private void ThenReturnNotFound(object response)
        {
            Assert.That(response, Is.InstanceOf<NotFoundResponse>());
        }

        private void ThenReturnInvalidPaymentStatusError(object response)
        {
            AssertSingleError((Response)response, "This payment status does not exist.");
            BusinessRepository.WasSetBookingPaymentStatusCalled = false;
        }

        private void ThenUpdateBookingPaymentStatus(object response)
        {
            BusinessRepository.WasSetBookingPaymentStatusCalled = true;
        }
    }
}
