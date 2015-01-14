using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Application.Configuration;
using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Authentication.Repositories;
using CoachSeek.DataAccess.Configuration;
using CoachSeek.DataAccess.Repositories;
using CoachSeek.Domain.Entities;
using NUnit.Framework;

namespace CoachSeek.Application.Tests.Unit
{
    public abstract class UseCaseTests
    {
        protected const string INVALID_BUSINESS_ID = "0031E489-C1D4-4889-A3E0-196930BA35DD";
        protected const string BUSINESS_ID = "10738006-7E54-4FA6-9CF5-7FEAB5940668";
        protected const string COACH_ALBERT_ID = "20B57B55-5E79-4F0D-9ACE-49FE30718CAF";
        protected const string COACH_BILL_ID = "30FF663E-C858-444B-800D-268D61F17E43";
        protected const string LOCATION_BROWNS_BAY_ID = "40318FFD-CB3E-46FE-9DDF-A2D03854DB69";
        protected const string LOCATION_ORAKEI_ID = "504E832A-F051-4FF8-86A4-829054E3BF93";
        protected const string SERVICE_MINI_RED_ID = "60D57C9C-58B0-4A10-94CA-2B12EBBF13AE";
        protected const string SERVICE_MINI_ORANGE_ID = "7018EC02-67B0-4F51-BD1A-A0D2E395D8FC";

        protected InMemoryUserRepository UserRepository { get; set; }
        protected InMemoryBusinessRepository BusinessRepository { get; set; }

        protected void ConfigureAutoMapper()
        {
            ApplicationAutoMapperConfigurator.Configure();
            DbAutoMapperConfigurator.Configure();
        }

        protected void SetupUserRepository()
        {
            UserRepository = new InMemoryUserRepository();
            UserRepository.Clear();

            var users = SetupUsers();
            UserRepository.Add(users);

            UserRepository.WasSaveNewUserCalled = false;
            UserRepository.WasSaveUserCalled = false;
        }

        protected void SetupBusinessRepository()
        {
            BusinessRepository = new InMemoryBusinessRepository();
            BusinessRepository.Clear();

            var business = SetupBusiness();
            BusinessRepository.Add(business);

            BusinessRepository.WasSaveNewBusinessCalled = false;
            BusinessRepository.WasSaveBusinessCalled = false;
        }

        protected IList<User> SetupUsers()
        {
            return new List<User> 
            {
                new User(Guid.NewGuid(), Guid.NewGuid(), "my business", "bgates@gmail.com", "William", "Gates", "bgates@gmail.com", "Microsoft75")
            };
        }

        protected Business SetupBusiness()
        {
            return new Business(new Guid(BUSINESS_ID),
                "Olaf's Tennis Coaching",
                "olafstenniscoaching",
                SetupLocations(),
                SetupCoaches(),
                SetupServices(),
                null);
        }

        protected IEnumerable<LocationData> SetupLocations()
        {
            return new List<LocationData>
            {
                 SetupBrownsBayLocation(),
                 SetupOrakeiLocation()
            };
        }

        protected IEnumerable<CoachData> SetupCoaches()
        {
            return new List<CoachData>
            {
                SetupCoachAlbert(),
                SetupCoachBill()
            };
        }

        protected IEnumerable<ServiceData> SetupServices()
        {
            return new List<ServiceData>
            {
                SetupServiceMiniRed(),
                SetupServiceMiniOrange()
            };
        }

        protected IEnumerable<SessionData> SetupSessions()
        {
            return new List<SessionData>
            {
                SetupSessionAlbertMiniRedBrownsBayTuesday14To15()
            };
        }

        private LocationData SetupBrownsBayLocation()
        {
            return new LocationData { Id = new Guid(LOCATION_BROWNS_BAY_ID), Name = "Browns Bay Racquets Club" };
        }

        protected LocationData SetupOrakeiLocation()
        {
            return new LocationData { Id = new Guid(LOCATION_ORAKEI_ID), Name = "Orakei Tennis Club" };
        }

        protected CoachData SetupCoachAlbert()
        {
            return new CoachData
            {
                Id = new Guid(COACH_ALBERT_ID),
                FirstName = "Albert",
                LastName = "Einstein",
                Email = "albert@relativity.net",
                Phone = "299784",
                WorkingHours = SetupStandardWorkingHoursData()
            };
        }

        protected CoachData SetupCoachBill()
        {
            return new CoachData
            {
                Id = new Guid(COACH_BILL_ID),
                FirstName = "Bill",
                LastName = "Gates",
                Email = "bill@microsoft.com",
                Phone = "095286912",
                WorkingHours = SetupWeekendWorkingHoursData()
            };
        }

        protected WeeklyWorkingHoursData SetupStandardWorkingHoursData()
        {
            return new WeeklyWorkingHoursData
            {
                Monday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                Tuesday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                Wednesday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                Thursday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                Friday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                Saturday = new DailyWorkingHoursData(false),
                Sunday = new DailyWorkingHoursData(false)
            };
        }

        protected WeeklyWorkingHoursData SetupWeekendWorkingHoursData()
        {
            return new WeeklyWorkingHoursData
            {
                Monday = new DailyWorkingHoursData(false),
                Tuesday = new DailyWorkingHoursData(false),
                Wednesday = new DailyWorkingHoursData(false),
                Thursday = new DailyWorkingHoursData(false),
                Friday = new DailyWorkingHoursData(false),
                Saturday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                Sunday = new DailyWorkingHoursData(true, "9:00", "17:00")
            };
        }

        private ServiceData SetupServiceMiniRed()
        {
            return new ServiceData
            {
                Id = new Guid(SERVICE_MINI_RED_ID),
                Name = "Mini Red",
                Repetition = new RepetitionData { SessionCount = 1 }
            };
        }

        private ServiceData SetupServiceMiniOrange()
        {
            return new ServiceData
            {
                Id = new Guid(SERVICE_MINI_ORANGE_ID),
                Name = "Mini Orange",
                Repetition = new RepetitionData { SessionCount = 1 }
            };
        }

        private SessionData SetupSessionAlbertMiniRedBrownsBayTuesday14To15()
        {
            return new SessionData
            {
            };
        }


        protected void AssertSaveBusinessIsNotCalled()
        {
            Assert.That(BusinessRepository.WasSaveBusinessCalled, Is.False);
        }

        protected void AssertSaveBusinessIsCalled()
        {
            Assert.That(BusinessRepository.WasSaveBusinessCalled, Is.True);
        }

        protected void AssertSingleError<TData>(Response<TData> response, 
                                                string expectedMessage, 
                                                string expectedField = null) where TData : class//, IData
        {
            Assert.That(response.Data, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Message, Is.EqualTo(expectedMessage));
            Assert.That(error.Field, Is.EqualTo(expectedField));
        }

        protected void AssertError(ErrorData error,
                                   string expectedMessage,
                                   string expectedField = null)
        {
            Assert.That(error.Message, Is.EqualTo(expectedMessage));
            Assert.That(error.Field, Is.EqualTo(expectedField));
        }
    }
}
