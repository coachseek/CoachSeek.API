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
            var response = WhenConstruct(sessionPricing);
            AssertSingleError(response, "The coursePrice field is not valid.", "session.pricing.coursePrice");
        }

        [Test]
        public void GivenCourseCanOnlyBePurchasedBySession_WhenConstruct_ThenConstructSessionPricing()
        {
            var sessionPricing = new PricingCommand(10, null);
            var response = WhenConstruct(sessionPricing);
            AssertSessionPricing(response, 10, null);
        }

        [Test]
        public void GivenCourseCanOnlyBePurchasedAsFullCourse_WhenConstruct_ThenConstructSessionPricing()
        {
            var sessionPricing = new PricingCommand(null, 100);
            var response = WhenConstruct(sessionPricing);
            AssertSessionPricing(response, null, 100);
        }

        [Test]
        public void GivenCourseCanBePurchasedBySessionOrCourse_WhenConstruct_ThenConstructSessionPricing()
        {
            var sessionPricing = new PricingCommand(10, 100);
            var response = WhenConstruct(sessionPricing);
            AssertSessionPricing(response, 10, 100);
        }


        private object WhenConstruct(PricingCommand sessionPricing)
        {
            try
            {
                return new RepeatedSessionPricing(sessionPricing);
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
