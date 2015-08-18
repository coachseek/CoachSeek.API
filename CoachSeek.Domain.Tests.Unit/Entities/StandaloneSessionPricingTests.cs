using CoachSeek.Common;
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
            AssertSingleError(response, 
                              ErrorCodes.SessionPriceInvalid,
                              "A SessionPrice of -10 is not valid.", 
                              "-10");
        }

        [Test]
        public void GivenCoursePrice_WhenConstruct_ThenThrowValidationException()
        {
            // A StandaloneSession is standalone and so should never have a CoursePrice passed in.
            var sessionPricing = new PricingCommand(10, 100);
            var response = WhenConstruct(sessionPricing);
            AssertSingleError(response,
                              ErrorCodes.StandaloneSessionMustHaveNoCoursePrice,
                              "Standalone sessions must not have the CoursePrice set.", 
                              null);
        }

        [Test]
        public void GivenMultipleErrorsInSessionPricing_WhenConstruct_ThenThrowValidationExceptionWithMultipleErrors()
        {
            var sessionPricing = new PricingCommand(-10, 100);
            var response = WhenConstruct(sessionPricing);
            AssertMultipleErrors(response, new[,] { { ErrorCodes.SessionPriceInvalid, "A SessionPrice of -10 is not valid.", "-10", null },
                                                    { ErrorCodes.StandaloneSessionMustHaveNoCoursePrice, "Standalone sessions must not have the CoursePrice set.", null, null } });
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
