using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class ServiceTests
    {
        [Test]
        public void GivenServiceMissingRepetition_WhenConstruct_ThenThrowValidationException()
        {
            var data = GivenServiceMissingRepetition();
            var response = WhenConstruct(data);
            AssertSingleError(response, "The repetition field is required.", "service.repetition");
        }

        [Test]
        public void GivenSingleSessionServiceAndHaveCoursePrice_WhenConstruct_ThenThrowValidationException()
        {
            var data = GivenSingleSessionServiceAndHaveCoursePrice();
            var service = WhenConstruct(data);
            AssertSingleError(service, "The coursePrice cannot be specified if the service is not for a course or is open-ended.", "service.pricing.coursePrice");
        }

        [Test]
        public void GivenOpenEndedCourseServiceAndHaveCoursePrice_WhenConstruct_ThenThrowValidationException()
        {
            var data = GivenOpenEndedCourseServiceAndHaveCoursePrice();
            var service = WhenConstruct(data);
            AssertSingleError(service, "The coursePrice cannot be specified if the service is not for a course or is open-ended.", "service.pricing.coursePrice");
        }


        private ServiceData GivenServiceMissingRepetition()
        {
             return new ServiceData
            {
                Name = "Mini Orange",
                Description = "Mini Orange Service",
            };
        }

        private ServiceData GivenSingleSessionServiceAndHaveCoursePrice()
        {
            return new ServiceData
            {
                Name = "Mini Orange",
                Description = "Mini Orange Service",
                Repetition = new RepetitionData
                {
                    RepeatTimes = 1
                },
                Pricing = new PricingData
                {
                    CoursePrice = 120
                }
            };
        }

        private ServiceData GivenOpenEndedCourseServiceAndHaveCoursePrice()
        {
            return new ServiceData
            {
                Name = "Mini Orange",
                Description = "Mini Orange Service",
                Repetition = new RepetitionData
                {
                    RepeatTimes = -1,
                    RepeatFrequency = "w"
                },
                Pricing = new PricingData
                {
                    CoursePrice = 120
                }
            };
        }


        private object WhenConstruct(ServiceData data)
        {
            try
            {
                return new Service(data);
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
            var errors = ((ValidationException) response).Errors;
            Assert.That(errors.Count, Is.EqualTo(1));
            var error = errors[0];
            Assert.That(error.Message, Is.EqualTo(message));
            Assert.That(error.Field, Is.EqualTo(field));
        }
    }
}
