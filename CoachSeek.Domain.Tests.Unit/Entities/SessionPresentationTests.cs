using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class SessionPresentationTests : Tests
    {
        private const string SERVICE_ID = "F1524EFA-C5A9-4731-8E56-E0BD6DC388D2";

        private ServiceData Service { get; set; }

        [SetUp]
        public void Setup()
        {
            Service = new ServiceData
            {
                Id = new Guid(SERVICE_ID),
                Name = "Mini Red",
                Description = "Mini Red Service",
                Repetition = new RepetitionData { RepeatTimes = 1 },
                Defaults = new ServiceDefaultsData { Duration = 105, Colour = "Red" },
                Booking = new ServiceBookingData { StudentCapacity = 17, IsOnlineBookable = true },
                Pricing = new PricingData { SessionPrice = 25 }
            };
        }


        [Test]
        public void GivenInvalidColour_WhenConstruct_ThenUseServiceBooking()
        {
            var sessionPresentation = new PresentationData {Colour = "aquamarine"};
            var response = WhenConstruct(sessionPresentation, null);
            AssertSingleError(response, "The colour is not valid.", "session.presentation.colour");
        }

        [Test]
        public void GivenServiceColourButNoSessionColour_WhenConstruct_ThenUseServiceColour()
        {
            var response = WhenConstruct(null, Service);
            AssertSessionPresentation(response, "red");
        }


        private object WhenConstruct(PresentationData sessionPresentation, ServiceData service)
        {
            try
            {
                return new SessionPresentation(sessionPresentation, service);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private void AssertSessionPresentation(object response, string colour)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<SessionPresentation>());
            var presentation = ((SessionPresentation)response);
            Assert.That(presentation.Colour, Is.EqualTo(colour));
        }
    }
}
