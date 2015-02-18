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
            var response = WhenConstruct(sessionPricing, null);
            AssertSingleError(response, "The coursePrice field is not valid.", "session.pricing.coursePrice");
        }

        [Test]
        public void GivenCourseCanOnlyBePurchasedBySession_WhenConstruct_ThenConstructSessionPricing()
        {
            var sessionPricing = new PricingCommand(10, null);
            var response = WhenConstruct(sessionPricing, null);
            AssertSessionPricing(response, 10, null);
        }

        [Test]
        public void GivenCourseCanOnlyBePurchasedAsFullCourse_WhenConstruct_ThenConstructSessionPricing()
        {
            var sessionPricing = new PricingCommand(null, 100);
            var response = WhenConstruct(sessionPricing, null);
            AssertSessionPricing(response, null, 100);
        }

        [Test]
        public void GivenCourseCanBePurchasedBySessionOrCourse_WhenConstruct_ThenConstructSessionPricing()
        {
            var sessionPricing = new PricingCommand(10, 100);
            var response = WhenConstruct(sessionPricing, null);
            AssertSessionPricing(response, 10, 100);
        }

        [Test]
        public void GivenMissingSessionPricing_WhenConstruct_ThenFallBackToServicePricing()
        {
            var servicePricing = new RepeatedSessionPricingData(15, 200);
            var response = WhenConstruct(null, servicePricing);
            AssertSessionPricing(response, 15, 200);
        }

        [Test]
        public void GivenOnePriceFromSessionAndOtherFromService_WhenConstruct_ThenGetValuesForBothPrices()
        {
            var servicePricing = new RepeatedSessionPricingData(15, null);
            var sessionPricing = new PricingCommand(null, 180);
            var response = WhenConstruct(sessionPricing, servicePricing);
            AssertSessionPricing(response, 15, 180);
        }

        [Test]
        public void GivenOnePriceFromServiceAndOtherFromSession_WhenConstruct_ThenGetValuesForBothPrices()
        {
            var servicePricing = new RepeatedSessionPricingData(null, 180);
            var sessionPricing = new PricingCommand(15, null);
            var response = WhenConstruct(sessionPricing, servicePricing);
            AssertSessionPricing(response, 15, 180);
        }

        [Test]
        public void GivenSessionPricesAndServicePrices_WhenConstruct_ThenSessionPricesOverrideServicePrices()
        {
            var servicePricing = new RepeatedSessionPricingData(10, 100);
            var sessionPricing = new PricingCommand(12, 150);
            var response = WhenConstruct(sessionPricing, servicePricing);
            AssertSessionPricing(response, 12, 150);
        }


        private object WhenConstruct(PricingCommand sessionPricing, RepeatedSessionPricingData servicePricing)
        {
            try
            {
                return new RepeatedSessionPricing(sessionPricing, servicePricing);
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
