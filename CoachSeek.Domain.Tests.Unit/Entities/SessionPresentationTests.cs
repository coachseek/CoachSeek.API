using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class SessionPresentationTests : Tests
    {
        private PresentationData ServicePresentation { get; set; }

        [SetUp]
        public void Setup()
        {
            ServicePresentation = new PresentationData {Colour = "Red"};
        }


        [Test]
        public void GivenInvalidColour_WhenConstruct_ThenUseServiceBooking()
        {
            var sessionPresentation = new PresentationCommand { Colour = "aquamarine" };
            var response = WhenConstruct(sessionPresentation, null);
            AssertSingleError(response, "The colour field is not valid.", "session.presentation.colour");
        }

        [Test]
        public void GivenServiceColourButNoSessionColour_WhenConstruct_ThenUseServiceColour()
        {
            var response = WhenConstruct(null, ServicePresentation);
            AssertSessionPresentation(response, "red");
        }


        private object WhenConstruct(PresentationCommand sessionPresentation, PresentationData servicePresentation)
        {
            try
            {
                return new SessionPresentation(sessionPresentation, servicePresentation);
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
