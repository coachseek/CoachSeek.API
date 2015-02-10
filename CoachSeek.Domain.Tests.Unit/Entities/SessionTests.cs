using System;
using CoachSeek.Application.Configuration;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Factories;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    public class SessionTests : Tests
    {
        private const string LOCATION_ID = "1F2A567D-58D2-4BE3-9AF4-2CEA7B477C83";
        private const string COACH_ID = "66045F50-4E8E-4ED4-B31F-A0740B25494F";
        private const string SERVICE_ID = "A8B901B6-A87B-49C4-B115-92182FA2DF6E";

        private LocationData Location { get; set; }
        private CoachData Coach { get; set; }
        private ServiceData Service { get; set; }


        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            ApplicationAutoMapperConfigurator.Configure();
        }

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
                Repetition = new RepetitionData { SessionCount = 1 },
                Timing = new ServiceTimingData { Duration = 105 },
                Booking = new ServiceBookingData { StudentCapacity = 17, IsOnlineBookable = true },
                Pricing = new PricingData { SessionPrice = 25 },
                Presentation = new PresentationData { Colour = "Red" },
            };
        }

        private SessionData CreateValidStandaloneSessionData()
        {
            return new SessionData
            {
                Location = new LocationKeyData { Id = new Guid(LOCATION_ID) },
                Coach = new CoachKeyData { Id = new Guid(COACH_ID) },
                Service = new ServiceKeyData { Id = new Guid(SERVICE_ID) },
                Timing = new SessionTimingData { StartDate = GetDateFormatOneWeekOut(), StartTime = "12:30", Duration = 45 },
                Booking = new SessionBookingData { StudentCapacity = 12, IsOnlineBookable = true },
                Repetition = new RepetitionData { SessionCount = 1 },
                Pricing = new PricingData { SessionPrice = 15 },
                Presentation = new PresentationData { Colour = "Red" }
            };
        }

        private Session CreateStandaloneSession(CoachData coachData, string startTime, int duration, Guid id)
        {
            var data = new SessionData
            {
                Id = id,
                Location = new LocationKeyData { Id = new Guid(LOCATION_ID) },
                Coach = new CoachKeyData { Id = coachData.Id },
                Service = new ServiceKeyData { Id = new Guid(SERVICE_ID) },
                Timing = new SessionTimingData { StartDate = GetDateFormatOneWeekOut(), StartTime = startTime, Duration = duration },
                Booking = new SessionBookingData { StudentCapacity = 12, IsOnlineBookable = true },
                Repetition = new RepetitionData { SessionCount = 1 },
                Pricing = new PricingData { SessionPrice = 15 },
                Presentation = new PresentationData { Colour = "Red" }
            };

            return SessionFactory.CreateSession(data, Location, coachData, Service);
        }

        private Session CreateNewStandaloneSession(CoachData coachData, string startTime, int duration)
        {
            var data = new NewSessionData
            {
                Location = new LocationKeyData { Id = new Guid(LOCATION_ID) },
                Coach = new CoachKeyData { Id = coachData.Id },
                Service = new ServiceKeyData { Id = new Guid(SERVICE_ID) },
                Timing = new SessionTimingData { StartDate = GetDateFormatOneWeekOut(), StartTime = startTime, Duration = duration },
                Booking = new SessionBookingData { StudentCapacity = 12, IsOnlineBookable = true },
                Repetition = new RepetitionData { SessionCount = 1 },
                Pricing = new PricingData { SessionPrice = 15 },
                Presentation = new PresentationData { Colour = "Red" }
            };

            return SessionFactory.CreateNewSession(data, Location, coachData, Service);
        }

        private Session CreateRepeatedSession(string startDate, string startTime, int duration, string repeatFrequency)
        {
            var data = new SessionData
            {
                Location = new LocationKeyData { Id = new Guid(LOCATION_ID) },
                Coach = new CoachKeyData { Id = Coach.Id },
                Service = new ServiceKeyData { Id = new Guid(SERVICE_ID) },
                Timing = new SessionTimingData { StartDate = startDate, StartTime = startTime, Duration = duration },
                Booking = new SessionBookingData { StudentCapacity = 12, IsOnlineBookable = true },
                Repetition = new RepetitionData { SessionCount = 10, RepeatFrequency = repeatFrequency},
                Pricing = new PricingData { SessionPrice = 15 },
                Presentation = new PresentationData { Colour = "Red" }
            };

            return SessionFactory.CreateSession(data, Location, Coach, Service);
        }


        [TestFixture]
        public class StandaloneSessionTests : SessionTests
        {
            [Test]
            public void GivenMultipleErrorsInStandaloneSession_WhenConstruct_ThenThrowValidationExceptionWithMultipleErrors()
            {
                Coach = null;
                Service.Booking.IsOnlineBookable = null;

                var session = CreateValidStandaloneSessionData();
                session.Timing.StartDate = "2014-02-30";
                session.Timing.Duration = 25;
                session.Booking.IsOnlineBookable = null;
                session.Pricing.CoursePrice = 120;
                session.Presentation.Colour = "maroon";

                var response = WhenConstructStandaloneSession(session);

                AssertMultipleErrors(response, new[,]
                {
                    {"The coach field is required.", "session.coach"},
                    {"The startDate field is not valid.", "session.timing.startDate"},
                    {"The duration field is not valid.", "session.timing.duration"},
                    {"The isOnlineBookable field is required.", "session.booking.isOnlineBookable"},
                    {"The colour field is not valid.", "session.presentation.colour"},
                    {"The coursePrice field must not be specified for a single session.", "session.pricing.coursePrice"}
                });
            }

            [Test]
            public void GivenValidStandaloneSession_WhenConstruct_ThenConstructSession()
            {
                var session = CreateValidStandaloneSessionData();
                var response = WhenConstructStandaloneSession(session);
                AssertStandaloneSession(response);
            }

            [Test]
            public void GivenStandaloneSession_WhenCallSessionNameProperty_ThenReturnSessionName()
            {
                var sessionData = CreateValidStandaloneSessionData();
                var session = SessionFactory.CreateNewSession(sessionData, Location, Coach, Service);
                var name = session.Name;
                Assert.That(name, Is.EqualTo("Mini Red at Orakei Tennis Club with Bob Jones on " + GetDateFormatOneWeekOut() + " at 12:30"));
            }

            private object WhenConstructStandaloneSession(SessionData session)
            {
                try
                {
                    return SessionFactory.CreateNewSession(session, Location, Coach, Service);
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }

            private void AssertStandaloneSession(object response)
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response, Is.InstanceOf<StandaloneSession>());
                var session = ((StandaloneSession)response);

                Assert.That(session.Location.Id, Is.EqualTo(new Guid(LOCATION_ID)));
                Assert.That(session.Coach.Id, Is.EqualTo(new Guid(COACH_ID)));
                Assert.That(session.Service.Id, Is.EqualTo(new Guid(SERVICE_ID)));

                var timing = session.Timing;
                Assert.That(timing.StartDate, Is.EqualTo(GetDateFormatOneWeekOut()));
                Assert.That(timing.StartTime, Is.EqualTo("12:30"));
                Assert.That(timing.Duration, Is.EqualTo(45));

                var booking = session.Booking;
                Assert.That(booking.StudentCapacity, Is.EqualTo(12));
                Assert.That(booking.IsOnlineBookable, Is.EqualTo(true));

                var pricing = session.Pricing;
                Assert.That(pricing.SessionPrice, Is.EqualTo(15));
                Assert.That(pricing.CoursePrice, Is.Null);

                var presentation = session.Presentation;
                Assert.That(presentation.Colour, Is.EqualTo("red"));
            }
        }


        [TestFixture]
        public class SingleSessionTests : SessionTests
        {
            [Test]
            public void GivenOtherSessionIsNull_WhenCallIsOverlapping_ThenReturnFalse()
            {
                var session = ConstructStandaloneSession(CreateValidStandaloneSessionData());
                var response = WhenCallIsOverlapping(session, null);
                Assert.That(response, Is.False);
            }

            [Test]
            public void GivenSameSession_WhenCallIsOverlapping_ThenReturnFalse()
            {
                var session = ConstructStandaloneSession(CreateValidStandaloneSessionData());
                var response = WhenCallIsOverlapping(session, session);
                Assert.That(response, Is.False);
            }

            [Test]
            public void GivenSessionIsOverlappingStartOfOtherSession_WhenCallIsOverlapping_ThenReturnTrue()
            {
                var session = CreateNewStandaloneSession(Coach, "12:30", 45);
                var otherSession = CreateStandaloneSession(Coach, "13:00", 60, Guid.NewGuid());
                var response = WhenCallIsOverlapping(session, otherSession);
                Assert.That(response, Is.True);
            }

            [Test]
            public void GivenSessionIsOverlappingFinishOfOtherSession_WhenCallIsOverlapping_ThenReturnTrue()
            {
                var session = CreateNewStandaloneSession(Coach, "13:45", 45);
                var otherSession = CreateStandaloneSession(Coach, "13:00", 60, Guid.NewGuid());
                var response = WhenCallIsOverlapping(session, otherSession);
                Assert.That(response, Is.True);
            }

            [Test]
            public void GivenSessionIsSpannedByOtherSession_WhenCallIsOverlapping_ThenReturnTrue()
            {
                var session = CreateNewStandaloneSession(Coach, "15:30", 30);
                var otherSession = CreateStandaloneSession(Coach, "15:00", 120, Guid.NewGuid());
                var response = WhenCallIsOverlapping(session, otherSession);
                Assert.That(response, Is.True);
            }

            [Test]
            public void GivenSessionIsSpanningOtherSession_WhenCallIsOverlapping_ThenReturnTrue()
            {
                var session = CreateNewStandaloneSession(Coach, "18:00", 30);
                var otherSession = CreateStandaloneSession(Coach, "17:45", 60, Guid.NewGuid());
                var response = WhenCallIsOverlapping(session, otherSession);
                Assert.That(response, Is.True);
            }

            [Test]
            public void GivenSessionIsNotOverlappingOtherSession_WhenCallIsOverlapping_ThenReturnFalse()
            {
                var session = CreateNewStandaloneSession(Coach, "12:45", 30);
                var otherSession = CreateStandaloneSession(Coach, "13:45", 45, Guid.NewGuid());
                var response = WhenCallIsOverlapping(session, otherSession);
                Assert.That(response, Is.False);
            }

            [Test]
            public void GivenSessionIsTouchingOtherSessionAtStart_WhenCallIsOverlapping_ThenReturnFalse()
            {
                var session = CreateNewStandaloneSession(Coach, "13:30", 30);
                var otherSession = CreateStandaloneSession(Coach, "14:00", 60, Guid.NewGuid());
                var response = WhenCallIsOverlapping(session, otherSession);
                Assert.That(response, Is.False);
            }

            [Test]
            public void GivenSessionIsTouchingOtherSessionAtFinish_WhenCallIsOverlapping_ThenReturnFalse()
            {
                var session = CreateNewStandaloneSession(Coach, "12:00", 60);
                var otherSession = CreateStandaloneSession(Coach, "11:00", 60, Guid.NewGuid());
                var response = WhenCallIsOverlapping(session, otherSession);
                Assert.That(response, Is.False);
            }


            private Session ConstructStandaloneSession(SessionData session)
            {
                var standaloneSession = SessionFactory.CreateNewSession(session, Location, Coach, Service);
                Assert.That(standaloneSession, Is.InstanceOf<StandaloneSession>());
                return standaloneSession;
            }
        }


        [TestFixture]
        public class RepeatedSessionTests : SessionTests
        {
            [Test]
            public void GivenRepeatedSessionIsOverlappingAnotherRepeatedSession_WhenCallIsOverlapping_ThenReturnTrue()
            {
                var session = CreateRepeatedSession(GetDateFormatNumberOfWeeksOut(2), "12:30", 45, "w");
                var otherSession = CreateRepeatedSession(GetDateFormatNumberOfWeeksOut(1), "13:00", 60, "2w");
                var response = WhenCallIsOverlapping(session, otherSession);
                Assert.That(response, Is.True);
            }
        }


        private object WhenCallIsOverlapping(Session session, Session otherSession)
        {
            try
            {
                return session.IsOverlapping(otherSession);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
