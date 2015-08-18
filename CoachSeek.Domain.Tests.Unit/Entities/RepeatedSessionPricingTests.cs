using CoachSeek.Common;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using NUnit.Framework;
using System;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class RepeatedSessionPricingTests : Tests
    {
        private RepetitionData Repetition { get; set; }

        [SetUp]
        public void Setup()
        {
            Repetition = new RepetitionData(10, "w");
        }

        [Test]
        public void GivenNegativeCoursePrice_WhenConstruct_ThenThrowValidationException()
        {
            var sessionPricing = new PricingCommand(null, -100);
            var response = WhenConstruct(sessionPricing, 12);
            AssertSingleError(response, 
                              ErrorCodes.CoursePriceInvalid,
                              "A CoursePrice of -100 is not valid.", 
                              "-100");
        }

        [Test]
        public void GivenCourseOnlyHasSessionPrice_WhenConstruct_ThenConstructCoursePriceFromSessionCountAndSessionPrice()
        {
            var sessionPricing = new PricingCommand(10, null);
            var response = WhenConstruct(sessionPricing, 12);
            AssertSessionPricing(response, 10, 120);
        }

        [Test]
        public void GivenCourseCanOnlyBePurchasedAsFullCourse_WhenConstruct_ThenConstructSessionPricing()
        {
            var sessionPricing = new PricingCommand(null, 100);
            var response = WhenConstruct(sessionPricing, 12);
            AssertSessionPricing(response, null, 100);
        }

        [Test]
        public void GivenCourseCanBePurchasedBySessionOrCourse_WhenConstruct_ThenConstructSessionPricing()
        {
            var sessionPricing = new PricingCommand(10, 100);
            var response = WhenConstruct(sessionPricing, 12);
            AssertSessionPricing(response, 10, 100);
        }


        private object WhenConstruct(PricingCommand sessionPricing, int sessionCount)
        {
            try
            {
                return new RepeatedSessionPricing(sessionPricing, sessionCount);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        private void AssertSessionPricing(object response, decimal? sessionPrice, decimal? coursePrice)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<RepeatedSessionPricing>());
            var pricing = ((RepeatedSessionPricing)response);
            Assert.That(pricing.SessionPrice, Is.EqualTo(sessionPrice));
            Assert.That(pricing.CoursePrice, Is.EqualTo(coursePrice));
        }
    }
}
