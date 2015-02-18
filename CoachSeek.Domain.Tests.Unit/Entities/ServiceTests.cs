using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class ServiceTests : Tests
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
                    SessionCount = 1
                },
                Pricing = new RepeatedSessionPricingData
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
                    SessionCount = -1,
                    RepeatFrequency = "w"
                },
                Pricing = new RepeatedSessionPricingData
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
    }
}
