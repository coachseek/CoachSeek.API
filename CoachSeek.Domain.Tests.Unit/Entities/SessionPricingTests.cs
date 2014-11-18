using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    public class SessionPricingTests
    {
        [TestFixture]
        public class SingleSessionPricingTests : SessionPricingTests
        {
            private RepetitionData Repetition { get; set; }

            [SetUp]
            public void Setup()
            {
                Repetition = new RepetitionData(1);
            }

            [Test]
            public void GivenNegativeSessionPrice_WhenConstruct_ThenThrowValidationException()
            {
                var sessionPricing = new PricingData(-10, null);
                var response = WhenConstruct(sessionPricing, null, Repetition);
                AssertSingleError(response, "The sessionPrice is not valid.", "session.pricing.sessionPrice");
            }

            [Test]
            public void GivenCoursePrice_WhenConstruct_ThenThrowValidationException()
            {
                var sessionPricing = new PricingData(10, 100);
                var response = WhenConstruct(sessionPricing, null, Repetition);
                AssertSingleError(response, "The coursePrice cannot be specified for a single session.", "session.pricing.coursePrice");
            }

            [Test]
            public void GivenSessionPrice_WhenConstruct_ThenConstructSessionPricing()
            {
                var sessionPricing = new PricingData(10, null);
                var response = WhenConstruct(sessionPricing, null, Repetition);
                AssertSessionPricing(response, 10, null);
            }

            [Test]
            public void GivenMissingPricing_WhenConstruct_ThenThrowValidationException()
            {
                var response = WhenConstruct(null, null, Repetition);
                AssertSingleError(response, "The pricing is required.", "session.pricing");
            }

            [Test]
            public void GivenMissingSessionPricing_WhenConstruct_ThenFallBackToServicePricing()
            {
                var servicePricing = new PricingData(15, null);
                var response = WhenConstruct(null, servicePricing, Repetition);
                AssertSessionPricing(response, 15, null);
            }
        }


        [TestFixture]
        public class CoursePricingTests : SessionPricingTests
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
                var sessionPricing = new PricingData(null, -100);
                var response = WhenConstruct(sessionPricing, null, Repetition);
                AssertSingleError(response, "The coursePrice is not valid.", "session.pricing.coursePrice");
            }

            [Test]
            public void GivenCourseCanOnlyBePurchasedBySession_WhenConstruct_ThenConstructSessionPricing()
            {
                var sessionPricing = new PricingData(10, null);
                var response = WhenConstruct(sessionPricing, null, Repetition);
                AssertSessionPricing(response, 10, null);
            }

            [Test]
            public void GivenCourseCanOnlyBePurchasedAsFullCourse_WhenConstruct_ThenConstructSessionPricing()
            {
                var sessionPricing = new PricingData(null, 100);
                var response = WhenConstruct(sessionPricing, null, Repetition);
                AssertSessionPricing(response, null, 100);
            }

            [Test]
            public void GivenCourseCanBePurchasedBySessionOrCourse_WhenConstruct_ThenConstructSessionPricing()
            {
                var sessionPricing = new PricingData(10, 100);
                var response = WhenConstruct(sessionPricing, null, Repetition);
                AssertSessionPricing(response, 10, 100);
            }

            [Test]
            public void GivenMissingSessionPricing_WhenConstruct_ThenFallBackToServicePricing()
            {
                var servicePricing = new PricingData(15, 200);
                var response = WhenConstruct(null, servicePricing, Repetition);
                AssertSessionPricing(response, 15, 200);
            }

            [Test]
            public void GivenOnePriceFromSessionAndOtherFromService_WhenConstruct_ThenGetValuesForBothPrices()
            {
                var servicePricing = new PricingData(15, null);
                var sessionPricing = new PricingData(null, 180);
                var response = WhenConstruct(sessionPricing, servicePricing, Repetition);
                AssertSessionPricing(response, 15, 180);
            }

            [Test]
            public void GivenOnePriceFromServiceAndOtherFromSession_WhenConstruct_ThenGetValuesForBothPrices()
            {
                var servicePricing = new PricingData(null, 180);
                var sessionPricing = new PricingData(15, null);
                var response = WhenConstruct(sessionPricing, servicePricing, Repetition);
                AssertSessionPricing(response, 15, 180);
            }

            [Test]
            public void GivenSessionPricesAndServicePrices_WhenConstruct_ThenSessionPricesOverrideServicePrices()
            {
                var servicePricing = new PricingData(10, 100);
                var sessionPricing = new PricingData(12, 150);
                var response = WhenConstruct(sessionPricing, servicePricing, Repetition);
                AssertSessionPricing(response, 12, 150);
            }
        }


        private object WhenConstruct(PricingData sessionPricing, PricingData servicePricing, RepetitionData repetition)
        {
            try
            {
                return new SessionPricing(sessionPricing, servicePricing, new SessionRepetition(repetition));
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private void AssertSingleError(object response, string message, string field)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = ((ValidationException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(1));
            var error = errors[0];
            Assert.That(error.Message, Is.EqualTo(message));
            Assert.That(error.Field, Is.EqualTo(field));
        }

        private void AssertSessionPricing(object response, decimal? sessionPrice, decimal? coursePrice)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<SessionPricing>());
            var pricing = ((SessionPricing)response);
            Assert.That(pricing.SessionPrice, Is.EqualTo(sessionPrice));
            Assert.That(pricing.CoursePrice, Is.EqualTo(coursePrice));
        }
    }
}
