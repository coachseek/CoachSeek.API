using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using NUnit.Framework;
using System;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class SingleSessionPricingTest : Tests
    {
        [Test]
        public void GivenNegativeSessionPrice_WhenConstruct_ThenThrowValidationException()
        {
            var sessionPricing = new PricingCommand(-10, null);
            var response = WhenConstruct(sessionPricing);
            AssertSingleError(response, "The sessionPrice field is not valid.", "session.pricing.sessionPrice");
        }

        [Test]
        public void GivenSessionPrice_WhenConstruct_ThenConstructSessionPricing()
        {
            var sessionPricing = new PricingCommand(10, null);
            var response = WhenConstruct(sessionPricing);
            AssertSessionPricing(response, 10);
        }

        [Test]
        public void GivenCoursePrice_WhenConstruct_ThenConstructSessionPricing()
        {
            // A SingleSession can be part of a RepeatedSession so a CoursePrice could be passed in.
            var sessionPricing = new PricingCommand(10, 100);
            var response = WhenConstruct(sessionPricing);
            AssertSessionPricing(response, 10);
        }

        //[Test]
        //public void GivenMissingPricing_WhenConstruct_ThenConstructWithoutPricing()
        //{
        //    var response = WhenConstruct(null, null);
        //    AssertSessionPricing(response, null);
        //}


        private object WhenConstruct(PricingCommand sessionPricing)
        {
            try
            {
                return new SingleSessionPricing(sessionPricing);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        private void AssertSessionPricing(object response, decimal? sessionPrice)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<SingleSessionPricing>());
            var pricing = ((SingleSessionPricing)response);
            Assert.That(pricing.SessionPrice, Is.EqualTo(sessionPrice));
        }
    }
}
