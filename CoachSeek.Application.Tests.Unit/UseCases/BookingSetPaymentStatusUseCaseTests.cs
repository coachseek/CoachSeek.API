using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Tests.Unit.Fakes;
using CoachSeek.Application.UseCases;
using CoachSeek.Common;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using NUnit.Framework;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class BookingSetPaymentStatusUseCaseTests : UseCaseTests
    {
        private BookingData BookingData { get; set; }
        private StubBookingGetByIdUseCase BookingGetByIdUseCase { get; set; }


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
            BookingData = null;

            return new BookingSetPaymentStatusCommand
            {
                BookingId = Guid.NewGuid(),
                PaymentStatus = Constants.PAYMENT_STATUS_PENDING_PAYMENT
            };
        }

        private BookingSetPaymentStatusCommand GivenNonExistentPaymentStatus()
        {
            BookingData = new SingleSessionBookingData { Id = new Guid(BOOKING_FRED_SESSION_TWO_ID) };

            return new BookingSetPaymentStatusCommand
            {
                BookingId = new Guid(BOOKING_FRED_SESSION_TWO_ID),
                PaymentStatus = "hello world!"
            };
        }

        private BookingSetPaymentStatusCommand GivenValidBookingSetPaymentStatusCommand()
        {
            BookingData = new SingleSessionBookingData { Id = new Guid(BOOKING_FRED_SESSION_TWO_ID) };

            return new BookingSetPaymentStatusCommand
            {
                BookingId = new Guid(BOOKING_FRED_SESSION_TWO_ID),
                PaymentStatus = Constants.PAYMENT_STATUS_PAID
            };
        }

        
        private object WhenTrySetPaymentStatus(BookingSetPaymentStatusCommand command)
        {
            BookingGetByIdUseCase = new StubBookingGetByIdUseCase(BookingData);
            var useCase = new BookingSetPaymentStatusUseCase(BookingGetByIdUseCase);
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
            Assert.That(BookingGetByIdUseCase.WasGetBookingCalled, Is.True);

            Assert.That(response, Is.InstanceOf<NotFoundResponse>());
        }

        private void ThenReturnInvalidPaymentStatusError(object response)
        {
            AssertSingleError((IResponse)response, "This payment status does not exist.");

            Assert.That(BookingGetByIdUseCase.WasGetBookingCalled, Is.True);
            Assert.That(BusinessRepository.WasSetBookingPaymentStatusCalled, Is.False);
        }

        private void ThenUpdateBookingPaymentStatus(object response)
        {
            Assert.That(BookingGetByIdUseCase.WasGetBookingCalled, Is.True);
            Assert.That(BusinessRepository.WasSetBookingPaymentStatusCalled, Is.True);
        }
    }
}
