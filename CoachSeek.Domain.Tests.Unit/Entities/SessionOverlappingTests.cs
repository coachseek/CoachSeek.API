using System;
using CoachSeek.Application.Configuration;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    public class SessionOverlappingTests
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


        private SingleSession CreateSingleSession(Guid sessionId, Guid coachId, Guid locationId, Guid serviceId, string startTime, int duration, int bookUntil)
        {
            var data = new SingleSessionData
            {
                Id = sessionId,
                Coach = new CoachKeyData { Id = coachId },
                Location = new LocationKeyData { Id = locationId },
                Service = new ServiceKeyData { Id = serviceId },
                Timing = new SessionTimingData("2015-04-01", startTime, duration, bookUntil),
                Booking = new SessionBookingData(10, true),
                Pricing = new SingleSessionPricingData(25),
                Repetition = new SingleRepetitionData(),
                Presentation = new PresentationData { Colour = "blue" }
            };

            var coreData = new CoreData
            {
                Coach = new CoachData { Id = coachId, FirstName = "a", LastName = "b", Email = "a@b.c", Phone = "1", WorkingHours = SetupWorkingHours() },
                Location = new LocationData { Id = locationId, Name = "here" },
                Service = new ServiceData { Id = serviceId, Name = "blowjob", Repetition = new RepetitionData(1)}
            };

            return new SingleSession(data, coreData);
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
        public class MovingSameSessionOverlappingTests : SessionOverlappingTests
        {
            [Test]
            public void MovedSessionToBeforeSessionNotTouching()
            {
                var sessionId = Guid.NewGuid();
                var sessionNow = CreateSingleSession(sessionId, ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);
                var sessionMoved = CreateSingleSession(sessionId, ZoeId, WaitomoId, MiniBlackId, "8:00", 60, 100);

                Assert.That(sessionNow.IsOverlapping(sessionMoved), Is.False);
                Assert.That(sessionMoved.IsOverlapping(sessionNow), Is.False);
            }

            [Test]
            public void MovedSessionToBeforeSessionButTouching()
            {
                var sessionId = Guid.NewGuid();
                var sessionNow = CreateSingleSession(sessionId, ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);
                var sessionMoved = CreateSingleSession(sessionId, ZoeId, WaitomoId, MiniBlackId, "9:00", 60, 100);

                Assert.That(sessionNow.IsOverlapping(sessionMoved), Is.False);
                Assert.That(sessionMoved.IsOverlapping(sessionNow), Is.False);
            }

            [Test]
            public void MovedSessionToBeforeSessionButOverlapping()
            {
                var sessionId = Guid.NewGuid();
                var sessionNow = CreateSingleSession(sessionId, ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);
                var sessionMoved = CreateSingleSession(sessionId, ZoeId, WaitomoId, MiniBlackId, "9:30", 60, 100);

                Assert.That(sessionNow.IsOverlapping(sessionMoved), Is.False);
                Assert.That(sessionMoved.IsOverlapping(sessionNow), Is.False);
            }

            [Test]
            public void SameSession()
            {
                var sessionId = Guid.NewGuid();
                var session = CreateSingleSession(sessionId, ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);

                Assert.That(session.IsOverlapping(session), Is.False);
            }

            [Test]
            public void MovedSessionToAfterSessionButOverlapping()
            {
                var sessionId = Guid.NewGuid();
                var sessionNow = CreateSingleSession(sessionId, ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);
                var sessionMoved = CreateSingleSession(sessionId, ZoeId, WaitomoId, MiniBlackId, "10:30", 60, 100);

                Assert.That(sessionNow.IsOverlapping(sessionMoved), Is.False);
                Assert.That(sessionMoved.IsOverlapping(sessionNow), Is.False);
            }

            [Test]
            public void MovedSessionToAfterSessionButTouching()
            {
                var sessionId = Guid.NewGuid();
                var sessionNow = CreateSingleSession(sessionId, ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);
                var sessionMoved = CreateSingleSession(sessionId, ZoeId, WaitomoId, MiniBlackId, "11:00", 60, 100);

                Assert.That(sessionNow.IsOverlapping(sessionMoved), Is.False);
                Assert.That(sessionMoved.IsOverlapping(sessionNow), Is.False);
            }

            [Test]
            public void MovedSessionToAfterSessionNotTouching()
            {
                var sessionId = Guid.NewGuid();
                var sessionNow = CreateSingleSession(sessionId, ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);
                var sessionMoved = CreateSingleSession(sessionId, ZoeId, WaitomoId, MiniBlackId, "12:00", 60, 100);

                Assert.That(sessionNow.IsOverlapping(sessionMoved), Is.False);
                Assert.That(sessionMoved.IsOverlapping(sessionNow), Is.False);
            }
        }


        [TestFixtureAttribute]
        public class DifferentCoachSessionOverlappingTests : SessionOverlappingTests
        {
            [Test]
            public void DifferentCoachSessionsNotTouching()
            {
                var session1 = CreateSingleSession(Guid.NewGuid(), ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);
                var session2 = CreateSingleSession(Guid.NewGuid(), YolandaId, WaitomoId, MiniBlackId, "8:00", 60, 100);

                Assert.That(session1.IsOverlapping(session2), Is.False);
                Assert.That(session2.IsOverlapping(session1), Is.False);
            }

            [Test]
            public void DifferentCoachSessionsTouching()
            {
                var session1 = CreateSingleSession(Guid.NewGuid(), ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);
                var session2 = CreateSingleSession(Guid.NewGuid(), YolandaId, WaitomoId, MiniBlackId, "9:00", 60, 100);

                Assert.That(session1.IsOverlapping(session2), Is.False);
                Assert.That(session2.IsOverlapping(session1), Is.False);
            }

            [Test]
            public void DifferentCoachSessionsPartiallyOverlapping()
            {
                var session1 = CreateSingleSession(Guid.NewGuid(), ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);
                var session2 = CreateSingleSession(Guid.NewGuid(), YolandaId, WaitomoId, MiniBlackId, "9:30", 60, 100);

                Assert.That(session1.IsOverlapping(session2), Is.False);
                Assert.That(session2.IsOverlapping(session1), Is.False);
            }

            [Test]
            public void DifferentCoachSessionsFullyOverlapping()
            {
                var session1 = CreateSingleSession(Guid.NewGuid(), ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);
                var session2 = CreateSingleSession(Guid.NewGuid(), YolandaId, WaitomoId, MiniBlackId, "10:00", 60, 100);

                Assert.That(session1.IsOverlapping(session2), Is.False);
                Assert.That(session2.IsOverlapping(session1), Is.False);
            }
        }


        [TestFixtureAttribute]
        public class DifferentLocationSessionOverlappingTests : SessionOverlappingTests
        {
            [Test]
            public void DifferentLocationSessionsNotTouching()
            {
                var session1 = CreateSingleSession(Guid.NewGuid(), ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);
                var session2 = CreateSingleSession(Guid.NewGuid(), ZoeId, PaeroaId, MiniBlackId, "8:00", 60, 100);

                Assert.That(session1.IsOverlapping(session2), Is.False);
                Assert.That(session2.IsOverlapping(session1), Is.False);
            }

            [Test]
            public void DifferentLocationSessionsTouching()
            {
                var session1 = CreateSingleSession(Guid.NewGuid(), ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);
                var session2 = CreateSingleSession(Guid.NewGuid(), ZoeId, PaeroaId, MiniBlackId, "9:00", 60, 100);

                Assert.That(session1.IsOverlapping(session2), Is.False);
                Assert.That(session2.IsOverlapping(session1), Is.False);
            }

            [Test]
            public void DifferentLocationSessionsPartiallyOverlapping()
            {
                var session1 = CreateSingleSession(Guid.NewGuid(), ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);
                var session2 = CreateSingleSession(Guid.NewGuid(), ZoeId, PaeroaId, MiniBlackId, "9:30", 60, 100);

                Assert.That(session1.IsOverlapping(session2), Is.True);
                Assert.That(session2.IsOverlapping(session1), Is.True);
            }

            [Test]
            public void DifferentLocationSessionsFullyOverlapping()
            {
                var session1 = CreateSingleSession(Guid.NewGuid(), ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);
                var session2 = CreateSingleSession(Guid.NewGuid(), ZoeId, PaeroaId, MiniBlackId, "10:00", 60, 100);

                Assert.That(session1.IsOverlapping(session2), Is.True);
                Assert.That(session2.IsOverlapping(session1), Is.True);
            }
        }


        [TestFixtureAttribute]
        public class DifferentServiceSessionOverlappingTests : SessionOverlappingTests
        {
            [Test]
            public void DifferentServiceSessionsNotTouching()
            {
                var session1 = CreateSingleSession(Guid.NewGuid(), ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);
                var session2 = CreateSingleSession(Guid.NewGuid(), ZoeId, WaitomoId, MiniWhiteId, "8:00", 60, 100);

                Assert.That(session1.IsOverlapping(session2), Is.False);
                Assert.That(session2.IsOverlapping(session1), Is.False);
            }

            [Test]
            public void DifferentServiceSessionsTouching()
            {
                var session1 = CreateSingleSession(Guid.NewGuid(), ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);
                var session2 = CreateSingleSession(Guid.NewGuid(), ZoeId, WaitomoId, MiniWhiteId, "9:00", 60, 100);

                Assert.That(session1.IsOverlapping(session2), Is.False);
                Assert.That(session2.IsOverlapping(session1), Is.False);
            }

            [Test]
            public void DifferentServiceSessionsPartiallyOverlapping()
            {
                var session1 = CreateSingleSession(Guid.NewGuid(), ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);
                var session2 = CreateSingleSession(Guid.NewGuid(), ZoeId, WaitomoId, MiniWhiteId, "9:30", 60, 100);

                Assert.That(session1.IsOverlapping(session2), Is.True);
                Assert.That(session2.IsOverlapping(session1), Is.True);
            }

            [Test]
            public void DifferentServiceSessionsFullyOverlapping()
            {
                var session1 = CreateSingleSession(Guid.NewGuid(), ZoeId, WaitomoId, MiniBlackId, "10:00", 60, 100);
                var session2 = CreateSingleSession(Guid.NewGuid(), ZoeId, WaitomoId, MiniWhiteId, "10:00", 60, 100);

                Assert.That(session1.IsOverlapping(session2), Is.True);
                Assert.That(session2.IsOverlapping(session1), Is.True);
            }
        }
    }
}
