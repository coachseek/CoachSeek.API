using CoachSeek.Application.Configuration;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Configuration;
using CoachSeek.DataAccess.Models;
using CoachSeek.DataAccess.Repositories;
using CoachSeek.WebUI.Controllers;
using CoachSeek.WebUI.Models.Api;
using CoachSeek.WebUI.Models.Api.Setup;
using NUnit.Framework;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.WebUI.Tests.Integration
{
    [TestFixture]
    public class CoachesControllerTests
    {
        private const string BUSINESS_ID = "12345678-B10E-4C0E-B46F-1386B98CE567";
        private const string ADMIN_ID = "D3F7C50D-5DBA-4787-B7A5-764BA140C319";
        private const string COACH_ID = "E8394D72-6960-44AE-BE44-79BCA3BC3A88";
        private const string LOCATION_ID = "52467312-087B-4F1C-B743-F55058FD0473";

        private CoachesController Controller { get; set; }

        private List<DbBusiness> Businesses
        {
            get { return InMemoryBusinessRepository.Businesses; }
        }

        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            WebApiAutoMapperConfigurator.Configure();
            ApplicationAutoMapperConfigurator.Configure();
            DbAutoMapperConfigurator.Configure();
        }

        [SetUp]
        public void Setup()
        {
            var container = new Container(new TypeRegistry());
            Controller =  container.GetInstance<CoachesController>();

            Controller.Request = new HttpRequestMessage();
            Controller.Configuration = new HttpConfiguration();

            SetupDatabase();
        }

        private void SetupDatabase()
        {
            Businesses.Clear();

            Businesses.Add(new DbBusiness
            {
                Id = new Guid(BUSINESS_ID),
                Name = "Olaf's Tennis Coaching",
                Domain = "olafstenniscoaching",
                Admin = new DbBusinessAdmin
                {
                    //BusinessId = new Guid(BUSINESS_ID),
                    Id = new Guid(ADMIN_ID),
                    FirstName = "Olaf",
                    LastName = "Thielke",
                    Email = "olaf@gmail.com",
                    Username = "olaf@gmail.com",
                    PasswordHash = "PasswordHash",
                    PasswordSalt = "PasswordSalt"
                },
                Locations = new List<DbLocation>
                {
                    new DbLocation
                    {
                        //BusinessId = new Guid(BUSINESS_ID),
                        Id = new Guid(LOCATION_ID),
                        Name = "Orakei Tennis Club",
                    }
                },
                Coaches = new List<DbCoach>
                {
                    new DbCoach
                    {
                        //BusinessId = new Guid(BUSINESS_ID),
                        Id = new Guid(COACH_ID),
                        FirstName = "Steve",
                        LastName = "McQueen",
                        Email = "steve.mcqueen@thegreatescape.com",
                        Phone = "0900 STALAG LUFT 13",
                        WorkingHours = new DbWeeklyWorkingHours
                        {
                            Monday = new DbDailyWorkingHours(false),
                            Tuesday = new DbDailyWorkingHours(false),
                            Wednesday = new DbDailyWorkingHours(false),
                            Thursday = new DbDailyWorkingHours(false),
                            Friday = new DbDailyWorkingHours(false),
                            Saturday = new DbDailyWorkingHours(true, "10:00","17:30"),
                            Sunday = new DbDailyWorkingHours(true, "10:00","17:30")
                        }
                    }
                }
            });
        }

        [Test]
        public void GivenFullNewCoach_WhenPost_ThenCreateNewCoach()
        {
            var command = GivenFullNewCoach();
            var response = WhenPost(command);
            ThenCreateNewCoach(response);
        }

        private ApiCoachSaveCommand GivenFullNewCoach()
        {
            return new ApiCoachSaveCommand
            {
                BusinessId = new Guid(BUSINESS_ID),
                FirstName = "Olaf",
                LastName = "Thielke",
                Email = "olaf@gmail.com",
                Phone = "0987654321",
                WorkingHours = SetupApiStandardWeeklyWorkingHours()
            };
        }

        private ApiWeeklyWorkingHours SetupApiStandardWeeklyWorkingHours()
        {
            return new ApiWeeklyWorkingHours
            {
                Monday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                Tuesday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                Wednesday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                Thursday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                Friday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                Saturday = new ApiDailyWorkingHours(false, null, null),
                Sunday = new ApiDailyWorkingHours(false, null, null)
            };
        }

        private HttpResponseMessage WhenPost(ApiCoachSaveCommand command)
        {
            return Controller.Post(command);
        }

        private void ThenCreateNewCoach(HttpResponseMessage response)
        {
            var businessData = AssertFullNewCoachResponse(response);
            AssertFullNewCoachIsPersisted(businessData);
        }

        private CoachData AssertFullNewCoachResponse(HttpResponseMessage response)
        {
            CoachData coach;
            Assert.That(response.TryGetContentValue(out coach), Is.True);

            AssertNewCoach(coach, "Olaf", "Thielke", "olaf@gmail.com", "0987654321");
            AssertStandardWorkingHours(coach);

            return coach;
        }

        private static void AssertNewCoach(CoachData coach, string firstName, string lastName, string email, string phone)
        {
            Assert.That(coach.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(coach.FirstName, Is.EqualTo(firstName));
            Assert.That(coach.LastName, Is.EqualTo(lastName));
            Assert.That(coach.Name, Is.EqualTo(string.Format("{0} {1}", firstName, lastName)));
            Assert.That(coach.Email, Is.EqualTo(email));
            Assert.That(coach.Phone, Is.EqualTo(phone));
        }

        private void AssertFullNewCoachIsPersisted(CoachData coach)
        {
            var dbBusiness = Businesses.Single(x => x.Id == new Guid(BUSINESS_ID));
            var dbCoach = dbBusiness.Coaches.Single(x => x.Id == coach.Id);

            Assert.That(dbCoach, Is.Not.Null);
            Assert.That(dbCoach.FirstName, Is.EqualTo(coach.FirstName));
            Assert.That(dbCoach.LastName, Is.EqualTo(coach.LastName));
            Assert.That(dbCoach.Email, Is.EqualTo(coach.Email));
            Assert.That(dbCoach.Phone, Is.EqualTo(coach.Phone));
            AssertStandardWorkingHours(dbCoach);
        }

        protected void AssertStandardWorkingHours(CoachData coach)
        {
            Assert.That(coach.WorkingHours, Is.Not.Null);
            AssertWorkingDay(coach.WorkingHours.Monday);
            AssertWorkingDay(coach.WorkingHours.Tuesday);
            AssertWorkingDay(coach.WorkingHours.Wednesday);
            AssertWorkingDay(coach.WorkingHours.Thursday);
            AssertWorkingDay(coach.WorkingHours.Friday);
            AssertNonWorkingDay(coach.WorkingHours.Saturday);
            AssertNonWorkingDay(coach.WorkingHours.Sunday);
        }

        protected void AssertWeekendWorkingHours(CoachData coach)
        {
            Assert.That(coach.WorkingHours, Is.Not.Null);
            AssertNonWorkingDay(coach.WorkingHours.Monday);
            AssertNonWorkingDay(coach.WorkingHours.Tuesday);
            AssertNonWorkingDay(coach.WorkingHours.Wednesday);
            AssertNonWorkingDay(coach.WorkingHours.Thursday);
            AssertNonWorkingDay(coach.WorkingHours.Friday);
            AssertWorkingDay(coach.WorkingHours.Saturday);
            AssertWorkingDay(coach.WorkingHours.Sunday);
        }

        private void AssertWorkingDay(DailyWorkingHoursData day)
        {
            Assert.That(day.IsAvailable, Is.True);
            Assert.That(day.StartTime, Is.EqualTo("9:00"));
            Assert.That(day.FinishTime, Is.EqualTo("17:00"));
        }

        private void AssertNonWorkingDay(DailyWorkingHoursData day)
        {
            Assert.That(day.IsAvailable, Is.False);
            Assert.That(day.StartTime, Is.Null);
            Assert.That(day.FinishTime, Is.Null);
        }

        protected void AssertStandardWorkingHours(DbCoach coach)
        {
            Assert.That(coach.WorkingHours, Is.Not.Null);
            AssertWorkingDay(coach.WorkingHours.Monday);
            AssertWorkingDay(coach.WorkingHours.Tuesday);
            AssertWorkingDay(coach.WorkingHours.Wednesday);
            AssertWorkingDay(coach.WorkingHours.Thursday);
            AssertWorkingDay(coach.WorkingHours.Friday);
            AssertNonWorkingDay(coach.WorkingHours.Saturday);
            AssertNonWorkingDay(coach.WorkingHours.Sunday);
        }

        private void AssertWorkingDay(DbDailyWorkingHours day)
        {
            Assert.That(day.IsAvailable, Is.True);
            Assert.That(day.StartTime, Is.EqualTo("9:00"));
            Assert.That(day.FinishTime, Is.EqualTo("17:00"));
        }

        private void AssertNonWorkingDay(DbDailyWorkingHours day)
        {
            Assert.That(day.IsAvailable, Is.False);
            Assert.That(day.StartTime, Is.Null);
            Assert.That(day.FinishTime, Is.Null);
        }
    }
}
