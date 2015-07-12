using System;
using System.Collections.Generic;
using CoachSeek.Application.Configuration;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    public class CourseOverlappingTests
    {
        private const string ZOE_ID = "F4A7449B-807F-4D4A-B71B-A4B785BEBD9F";
        private const string YOLANDA_ID = "E320D70B-D919-4B45-BC2F-E0653280D1E4";
        private const string WAITOMO_ID = "D22359F5-8AE8-4CCC-A443-7A6BBB3D35F8";
        private const string PAEROA_ID = "C13F64AB-0DB9-4A08-88BC-06636A23BBDE";
        private const string MINI_BLACK_ID = "B049FA25-B273-40A6-A944-933385F599A9";
        private const string MINI_WHITE_ID = "AAFFE130-2AFA-4E80-A9A8-2040F4D3331A";

        private Guid ZoeId { get { return new Guid(ZOE_ID); } }
        private Guid YolandaId { get { return new Guid(YOLANDA_ID); } }
        private Guid WaitomoId { get { return new Guid(WAITOMO_ID); } }
        private Guid PaeroaId { get { return new Guid(PAEROA_ID); } }
        private Guid MiniBlackId { get { return new Guid(MINI_BLACK_ID); } }
        private Guid MiniWhiteId { get { return new Guid(MINI_WHITE_ID); } }


        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            ApplicationAutoMapperConfigurator.Configure();
        }


        protected RepeatedSession CreateCourse(Guid courseId, Guid coachId, Guid locationId, Guid serviceId, string startTime, int duration)
        {
            var data = new RepeatedSessionData
            {
                Id = courseId,
                Coach = new CoachKeyData { Id = coachId },
                Location = new LocationKeyData { Id = locationId },
                Service = new ServiceKeyData { Id = serviceId },
                Timing = new SessionTimingData("2015-05-01", startTime, duration),
                Booking = new SessionBookingData(10, true),
                Pricing = new RepeatedSessionPricingData(25, 50),
                Repetition = new RepetitionData(2, "d"),
                Presentation = new PresentationData { Colour = "blue" },
                Sessions = new List<SingleSessionData>
                {
                    new SingleSessionData
                    {
                        ParentId = courseId,
                        Id = Guid.NewGuid(),
                        Coach = new CoachKeyData { Id = coachId },
                        Location = new LocationKeyData { Id = locationId },
                        Service = new ServiceKeyData { Id = serviceId },
                        Timing = new SessionTimingData("2015-05-01", startTime, duration),
                        Booking = new SessionBookingData(10, true),
                        Pricing = new SingleSessionPricingData(25),
                        Repetition = new SingleRepetitionData(),
                        Presentation = new PresentationData { Colour = "blue" }
                    },
                    new SingleSessionData
                    {
                        ParentId = courseId,
                        Id = Guid.NewGuid(),
                        Coach = new CoachKeyData { Id = coachId },
                        Location = new LocationKeyData { Id = locationId },
                        Service = new ServiceKeyData { Id = serviceId },
                        Timing = new SessionTimingData("2015-05-02", startTime, duration),
                        Booking = new SessionBookingData(10, true),
                        Pricing = new SingleSessionPricingData(25),
                        Repetition = new SingleRepetitionData(),
                        Presentation = new PresentationData { Colour = "blue" }
                    }
                }
            };

            var coach = new CoachData
            {
                Id = coachId,
                FirstName = "a",
                LastName = "b",
                Email = "a@b.c",
                Phone = "1",
                WorkingHours = SetupWorkingHours()
            };
            var location = new LocationData {Id = locationId, Name = "here"};
            var service = new ServiceData {Id = serviceId, Name = "blowjob", Repetition = new RepetitionData(1)};

            var coreData = new CoreData
            {
                Coach = coach,
                Location = location,
                Service = service
            };

            return new RepeatedSession(data, coreData, new[] { location }, new[] { coach }, new[] { service });
        }

        private WeeklyWorkingHoursData SetupWorkingHours()
        {
            return new WeeklyWorkingHoursData
            {
                Monday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                Tuesday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                Wednesday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                Thursday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                Friday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                Saturday = new DailyWorkingHoursData(),
                Sunday = new DailyWorkingHoursData()
            };
        }


        [TestFixtureAttribute]
        public class MovingSameSessionOverlappingTests : CourseOverlappingTests
        {
            [Test]
            public void SameCourseShouldBeOverlapping()
            {
                var course1 = CreateCourse(Guid.NewGuid(), ZoeId, WaitomoId, MiniBlackId, "10:00", 60);
                var course2 = CreateCourse(Guid.NewGuid(), ZoeId, WaitomoId, MiniBlackId, "10:00", 60);

                Assert.That(course1.IsOverlapping(course2), Is.True);
                Assert.That(course2.IsOverlapping(course1), Is.True);
            }
        }
    }
}
