using System;
using CoachSeek.Application.Configuration;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Services;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Services
{
    [TestFixture]
    public class SingleSessionListCalculatorTests : Tests
    {
        private const string LOCATION_ID = "1F2A567D-58D2-4BE3-1111-2CEA7B477C83";
        private const string COACH_ID = "66045F50-4E8E-4ED4-2222-A0740B25494F";
        private const string SERVICE_ID = "A8B901B6-A87B-49C4-3333-92182FA2DF6E";

        private LocationData Location { get; set; }
        private CoachData Coach { get; set; }
        private ServiceData Service { get; set; }

        private SingleSession FirstSession { get; set; }


        [SetUp]
        public void Setup()
        {
            ApplicationAutoMapperConfigurator.Configure();

            SetupLocation();
            SetupCoach();
            SetupService();

            FirstSession = new SingleSession(CreateValidSingleSession(), Location, Coach, Service);
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
                Repetition = new RepetitionData { SessionCount = 1 },
                Timing = new ServiceTimingData { Duration = 105 },
                Booking = new ServiceBookingData { StudentCapacity = 17, IsOnlineBookable = true },
                Pricing = new PricingData { SessionPrice = 25 },
                Presentation = new PresentationData { Colour = "Red" },
            };
        }

        private SessionData CreateValidSingleSession()
        {
            return new SessionData
            {
                Location = new LocationKeyData { Id = new Guid(LOCATION_ID) },
                Coach = new CoachKeyData { Id = new Guid(COACH_ID) },
                Service = new ServiceKeyData { Id = new Guid(SERVICE_ID) },
                Timing = new SessionTimingData { StartDate = GetDateFormatOneWeekOut(), StartTime = "13:45", Duration = 75 },
                Booking = new SessionBookingData { StudentCapacity = 12, IsOnlineBookable = true },
                Repetition = new RepetitionData { SessionCount = 1 },
                Pricing = new PricingData { SessionPrice = 16.5m },
                Presentation = new PresentationData { Colour = "Red" }
            };
        }


        [Test]
        public void TestDailySingleSessionListCalculator()
        {
            AssertDailySessions(2);
            AssertDailySessions(3);
            AssertDailySessions(5);
            AssertDailySessions(8);
            AssertDailySessions(13);
            AssertDailySessions(20);
            AssertDailySessions(40);
            AssertDailySessions(60);
            AssertDailySessions(100);
        }

        [Test]
        public void TestWeeklySingleSessionListCalculator()
        {
            AssertWeeklySessions(2);
            AssertWeeklySessions(3);
            AssertWeeklySessions(5);
            AssertWeeklySessions(8);
            AssertWeeklySessions(13);
            AssertWeeklySessions(20);
            AssertWeeklySessions(40);
            AssertWeeklySessions(60);
            AssertWeeklySessions(100);
        }


        private void AssertDailySessions(int sessionCount)
        {
            var calculator = new DailySingleSessionListCalculator();
            var sessions = calculator.Calculate(FirstSession, new SessionCount(sessionCount));

            Assert.That(sessions.Count, Is.EqualTo(sessionCount));
            AssertFirstSession(sessions[0]);
            for (var i = 1; i < sessionCount; i++)
                AssertSubsequentDailySession(sessions[i], i);
        }

        private void AssertWeeklySessions(int sessionCount)
        {
            var calculator = new WeeklySingleSessionListCalculator();
            var sessions = calculator.Calculate(FirstSession, new SessionCount(sessionCount));

            Assert.That(sessions.Count, Is.EqualTo(sessionCount));
            AssertFirstSession(sessions[0]);
            for (var i = 1; i < sessionCount; i++)
                AssertSubsequentWeeklySession(sessions[i], i);
        }


        private void AssertFirstSession(SingleSession firstSession)
        {
            Assert.That(firstSession, Is.Not.Null);
            Assert.That(firstSession.Id, Is.EqualTo(FirstSession.Id));


            AssertSession(firstSession, GetDateFormatOneWeekOut());
        }

        private void AssertSubsequentDailySession(SingleSession session, int sessionIndex)
        {
            Assert.That(session, Is.Not.Null);
            Assert.That(session.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(session.Id, Is.Not.EqualTo(FirstSession.Id));

            AssertSession(session, GetDateFormatNumberOfDaysOut(DAYS_IN_WEEK + sessionIndex));
        }

        private void AssertSubsequentSecondDaySession(SingleSession session, int sessionIndex)
        {
            Assert.That(session, Is.Not.Null);
            Assert.That(session.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(session.Id, Is.Not.EqualTo(FirstSession.Id));

            AssertSession(session, GetDateFormatNumberOfDaysOut(DAYS_IN_WEEK + 2 * sessionIndex));
        }

        private void AssertSubsequentWeeklySession(SingleSession session, int sessionIndex)
        {
            Assert.That(session, Is.Not.Null);
            Assert.That(session.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(session.Id, Is.Not.EqualTo(FirstSession.Id));

            AssertSession(session, GetDateFormatNumberOfDaysOut(DAYS_IN_WEEK * (sessionIndex + 1)));
        }

        private void AssertSubsequentFortnightlySession(SingleSession session, int sessionIndex)
        {
            Assert.That(session, Is.Not.Null);
            Assert.That(session.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(session.Id, Is.Not.EqualTo(FirstSession.Id));

            AssertSession(session, GetDateFormatNumberOfDaysOut(DAYS_IN_WEEK + 2 * DAYS_IN_WEEK * sessionIndex));
        }

        private void AssertSession(SingleSession session, string data)
        {
            var timing = session.Timing;
            Assert.That(timing, Is.Not.Null);
            Assert.That(timing.StartDate, Is.EqualTo(data));
            Assert.That(timing.StartTime, Is.EqualTo("13:45"));
            Assert.That(timing.Duration, Is.EqualTo(75));

            var booking = session.Booking;
            Assert.That(booking, Is.Not.Null);
            Assert.That(booking.StudentCapacity, Is.EqualTo(12));
            Assert.That(booking.IsOnlineBookable, Is.True);

            var presentation = session.Presentation;
            Assert.That(presentation, Is.Not.Null);
            Assert.That(presentation.Colour, Is.EqualTo("red"));

            var pricing = session.Pricing;
            Assert.That(pricing, Is.Not.Null);
            Assert.That(pricing.SessionPrice, Is.EqualTo(16.5m));
            Assert.That(pricing.CoursePrice, Is.Null);
        }
    }
}
