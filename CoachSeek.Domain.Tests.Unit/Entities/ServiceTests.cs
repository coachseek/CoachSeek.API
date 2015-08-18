using System;
using CoachSeek.Common;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class ServiceTests : Tests
    {
        [Test]
        public void GivenSingleSessionServiceAndHaveCoursePrice_WhenConstruct_ThenThrowValidationException()
        {
            var command = GivenSingleSessionServiceAndHaveCoursePrice();
            var service = WhenConstruct(command);
            AssertSingleError(service, 
                              ErrorCodes.ServiceForStandaloneSessionMustHaveNoCoursePrice,
                              "Services for standalone sessions must not have the CoursePrice set.", 
                              null);
        }


        private ServiceAddCommand GivenSingleSessionServiceAndHaveCoursePrice()
        {
            return new ServiceAddCommand
            {
                Name = "Mini Orange",
                Description = "Mini Orange Service",
                Repetition = new RepetitionCommand
                {
                    SessionCount = 1
                },
                Pricing = new PricingCommand
                {
                    CoursePrice = 120
                }
            };
        }


        private object WhenConstruct(ServiceAddCommand command)
        {
            try
            {
                return new Service(command);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
