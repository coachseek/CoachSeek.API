using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using NUnit.Framework;
using System;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class StandaloneSessionPricingTest : Tests
    {
        [Test]
        public void GivenNegativeSessionPrice_WhenConstruct_ThenThrowValidationException()
        {
            var sessionPricing = new PricingCommand(-10, null);
            var response = WhenConstruct(sessionPricing);
            AssertSingleError(response, "The sessionPrice field is not valid.", "session.pricing.sessionPrice");
        }

        [Test]
        public void GivenCoursePrice_WhenConstruct_ThenThrowValidationException()
        {
            // A StandaloneSession is standalone and so should never have a CoursePrice passed in.
            var sessionPricing = new PricingCommand(10, 100);
            var response = WhenConstruct(sessionPricing);
            AssertSingleError(response, "The coursePrice field must not be specified for a single session.", "session.pricing.coursePrice");
        }

        [Test]
        public void GivenMultipleErrorsInSessionPricing_WhenConstruct_ThenThrowValidationExceptionWithMultipleErrors()
        {
            var sessionPricing = new PricingCommand(-10, 100);
            var response = WhenConstruct(sessionPricing);
            AssertMultipleErrors(response, new[,] { { "The sessionPrice field is not valid.", "session.pricing.sessionPrice" },
                                                    { "The coursePrice field must not be specified for a single session.", "session.pricing.coursePrice" } });
        }

        [Test]
        public void GivenSessionPrice_WhenConstruct_ThenConstructSessionPricing()
        {
            var sessionPricing = new PricingCommand(10, null);
            var response = WhenConstruct(sessionPricing);
            AssertSessionPricing(response, 10, null);
        }

        //[Test]
        //public void GivenMissingPricing_WhenConstruct_ThenThrowValidationException()
        //{
        //    var response = WhenConstruct(null);
        //    AssertSingleError(response, "A sessionPrice is required.", "session.pricing.sessionPrice");
        //}


        private object WhenConstruct(PricingCommand sessionPricing)
        {
            try
            {
                return new StandaloneSessionPricing(sessionPricing);
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
