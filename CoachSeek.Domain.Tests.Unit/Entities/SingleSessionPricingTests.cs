using CoachSeek.Data.Model;
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
            var sessionPricing = new PricingData(-10, null);
            var response = WhenConstruct(sessionPricing, null);
            AssertSingleError(response, "The sessionPrice field is not valid.", "session.pricing.sessionPrice");
        }

        [Test]
        public void GivenCoursePrice_WhenConstruct_ThenThrowValidationException()
        {
            var sessionPricing = new PricingData(10, 100);
            var response = WhenConstruct(sessionPricing, null);
            AssertSingleError(response, "The coursePrice field must not be specified for a single session.", "session.pricing.coursePrice");
        }

        [Test]
        public void GivenMultipleErrorsInSessionPricing_WhenConstruct_ThenThrowValidationExceptionWithMultipleErrors()
        {
            var sessionPricing = new PricingData(-10, 100);
            var response = WhenConstruct(sessionPricing, null);
            AssertMultipleErrors(response, new[,] { { "The sessionPrice field is not valid.", "session.pricing.sessionPrice" },
                                                    { "The coursePrice field must not be specified for a single session.", "session.pricing.coursePrice" } });
        }

        [Test]
        public void GivenSessionPrice_WhenConstruct_ThenConstructSessionPricing()
        {
            var sessionPricing = new PricingData(10, null);
            var response = WhenConstruct(sessionPricing, null);
            AssertSessionPricing(response, 10, null);
        }

        [Test]
        public void GivenMissingPricing_WhenConstruct_ThenConstructWithoutPricing()
        {
            var response = WhenConstruct(null, null);
            AssertSessionPricing(response, null, null);
        }

        [Test]
        public void GivenMissingSessionPricing_WhenConstruct_ThenFallBackToServicePricing()
        {
            var servicePricing = new PricingData(15, null);
            var response = WhenConstruct(null, servicePricing);
            AssertSessionPricing(response, 15, null);
        }

        private object WhenConstruct(PricingData sessionPricing, PricingData servicePricing)
        {
            try
            {
                return new SingleSessionPricing(sessionPricing, servicePricing);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        private void AssertSessionPricing(object response, decimal? sessionPrice, decimal? coursePrice)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<SingleSessionPricing>());
            var pricing = ((SingleSessionPricing)response);
            Assert.That(pricing.SessionPrice, Is.EqualTo(sessionPrice));
        }
    }
}
