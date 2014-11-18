using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class SessionTests
    {
        private const string LOCATION_ID = "1F2A567D-58D2-4BE3-9AF4-2CEA7B477C83";
        private const string COACH_ID = "66045F50-4E8E-4ED4-B31F-A0740B25494F";
        private const string SERVICE_ID = "A8B901B6-A87B-49C4-B115-92182FA2DF6E";

        private LocationData Location { get; set; }
        private CoachData Coach { get; set; }
        private ServiceData Service { get; set; }

        [SetUp]
        public void Setup()
        {
            SetupLocation();
            SetupCoach();
            SetupService();
        }

        private void SetupLocation()
        {
            Location = new LocationData
            {
                Id = new Guid(LOCATION_ID),
                Name = "Orakei Tennis Club"
            };
        }

        private void SetupCoach()
        {
            Coach = new CoachData
            {
                Id = new Guid(COACH_ID),
                FirstName = "Bob",
                LastName = "Jones",
                Email = "bob@jones.com",
                Phone = "1234567890",
                WorkingHours = new WeeklyWorkingHoursData
                {
                    Monday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                    Tuesday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                    Wednesday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                    Thursday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                    Friday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                    Saturday = new DailyWorkingHoursData(false),
                    Sunday = new DailyWorkingHoursData(false)
                }
            };
        }

        private void SetupService()
        {
            Service = new ServiceData
            {
                Id = new Guid(SERVICE_ID),
                Name = "Mini Red",
                Description = "Mini Red Service",
                Repetition = new RepetitionData { RepeatTimes = 1 },
                Defaults = new ServiceDefaultsData { Duration = 105, Colour = "Red" },
                Booking = new ServiceBookingData { StudentCapacity = 17, IsOnlineBookable = true },
                Pricing = new PricingData { SessionPrice = 25 }
            };
        }


        private SessionData GivenServiceAndSessionMissingDuration()
        {
            Service.Defaults.Duration = null;

            return new SessionData
            {
                Location = new LocationKeyData {Id = new Guid(LOCATION_ID)},
                Coach = new CoachKeyData {Id = new Guid(COACH_ID)},
                Service = new ServiceKeyData {Id = new Guid(SERVICE_ID)},
                Timing = new SessionTimingData { StartDate = "2014-11-20", StartTime = "12:30" }
            };
        } 

        private object WhenConstruct(SessionData data)
        {
            try
            {
                return new Session(data, Location, Coach, Service);
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
