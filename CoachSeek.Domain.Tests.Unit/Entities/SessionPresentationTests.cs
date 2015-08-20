using System;
using CoachSeek.Common;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class SessionPresentationTests : Tests
    {
        [Test]
        public void GivenInvalidColour_WhenConstruct_ThenInvalidColourError()
        {
            var sessionPresentation = new PresentationCommand { Colour = "aquamarine" };
            var response = WhenConstruct(sessionPresentation);
            AssertSingleError(response,
                              ErrorCodes.ColourInvalid,
                              "Colour 'aquamarine' is not valid.",
                              "aquamarine");
        }

        [Test]
        public void GivenValidColour_WhenConstruct_ThenCreateColour()
        {
            var sessionPresentation = new PresentationCommand { Colour = "yellow" };
            var response = WhenConstruct(sessionPresentation);
            AssertSessionPresentation(response, "yellow");
        }


        private object WhenConstruct(PresentationCommand sessionPresentation)
        {
            try
            {
                return new SessionPresentation(sessionPresentation);
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
