using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.UseCases;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class BookingDeleteUseCaseTests : UseCaseTests
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
        public void GivenNonExistentBookingId_WhenTryDeleteBooking_ThenReturnNotFoundResponse()
        {
            var bookingId = GivenNonExistentBookingId();
            var response = WhenTryDeleteBooking(bookingId);
            ThenReturnNotFoundResponse(response);
        }

        [Test]
        public void GivenEmptyBookingId_WhenTryDeleteBooking_ThenReturnNotFoundResponse()
        {
            var bookingId = GivenEmptyBookingId();
            var response = WhenTryDeleteBooking(bookingId);
            ThenReturnNotFoundResponse(response);
        }

        [Test]
        public void GivenInvalidBookingId_WhenTryDeleteBooking_ThenReturnNotFoundResponse()
        {
            var bookingId = GivenInvalidBookingId();
            var response = WhenTryDeleteBooking(bookingId);
            ThenReturnNotFoundResponse(response);
        }

        [Test]
        public void GivenValidBookingId_WhenTryDeleteBooking_ThenDeleteBooking()
        {
            var bookingId = GivenValidBookingId();
            var response = WhenTryDeleteBooking(bookingId);
            ThenDeleteBooking(response);
        }

        private Guid GivenInvalidBookingId()
        {
            return new Guid(INVALID_BUSINESS_ID);
        }

        private Guid GivenValidBookingId()
        {
            return new Guid(BOOKING_FRED_SESSION_TWO_ID);
        }

        private Guid GivenEmptyBookingId()
        {
            return Guid.Empty;
        }

        private Guid GivenNonExistentBookingId()
        { 
            return Guid.NewGuid();
        }

        private object WhenTryDeleteBooking(Guid bookingId)
        {
            var booking = new BookingDeleteUseCase(new BookingGetByIdUseCase());
            var business = new BusinessDetails(new Guid(BUSINESS_ID), "", "");
            var currency = new CurrencyDetails("NZD", "$");
            var businessContext = new BusinessContext(business, currency, BusinessRepository, SupportedCurrencyRepository, UserRepository);
            var emailContext = new EmailContext(true, false, "", null);
            var context = new ApplicationContext(null, businessContext, emailContext, true);
            booking.Initialise(context);
            try
            {
                return booking.DeleteBooking(bookingId);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private void ThenReturnNotFoundResponse(object response)
        {
            Assert.That(response, Is.InstanceOf<NotFoundResponse>());
        }

        private void ThenDeleteBooking(object response)
        {
            Assert.That(BusinessRepository.WasDeleteBookingCalled, Is.True);  
        }
    }
}
