using CoachSeek.Application.Configuration;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Authentication.Repositories;
using CoachSeek.DataAccess.Main.Memory.Configuration;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using CoachSeek.Domain.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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
        protected const string SESSION_ONE = "1ABEBA68-8F90-45AB-8E56-C2B34E8DB657";
        protected const string SESSION_TWO = "0C2B472C-0DFA-4545-94B8-DE5A807FF52F";
        protected const string SESSION_THREE = "33648896-460D-409D-A2BA-D8F1EA852757";
        protected const string SESSION_FOUR = "B715BFC7-B9D4-4C67-8014-17B2825C081C";
        protected const string SESSION_FIVE = "B76E3493-01CA-40A5-8B90-B3157A7AAB21";

        protected InMemoryUserRepository UserRepository { get; set; }
        protected InMemoryBusinessRepository BusinessRepository { get; set; }
        protected HardCodedSupportedCurrencyRepository SupportedCurrencyRepository { get; set; }

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

            SetupBusiness();
            SetupLocations();
            SetupCoaches();
            SetupServices();
            SetupSessions();
            SetupCourses();
            SetupCustomers();

            BusinessRepository.WasAddBusinessCalled = false;
            BusinessRepository.WasAddLocationCalled = false;
            BusinessRepository.WasAddCoachCalled = false;
        }

        protected void SetupSupportedCurrencyRepository()
        {
            SupportedCurrencyRepository = new HardCodedSupportedCurrencyRepository();
        }

        protected IList<User> SetupUsers()
        {
            return new List<User> 
            {
                new User(Guid.NewGuid(), Guid.NewGuid(), "my business", "bgates@gmail.com", "William", "Gates", "bgates@gmail.com", "Microsoft75")
            };
        }

        private void SetupBusiness()
        {
            BusinessRepository.AddBusiness(new Business(new Guid(BUSINESS_ID), 
                                                        "Olaf's Tennis Coaching",
                                                        "olafstenniscoaching", "USD"));
        }

        protected void SetupLocations()
        {
            SetupBrownsBayLocation();
            SetupOrakeiLocation();
        }

        protected void SetupCoaches()
        {
            SetupCoachAlbert();
            SetupCoachBill();
        }

        protected void SetupServices()
        {
            SetupServiceMiniRed();
            SetupServiceMiniOrange();
        }

        protected void SetupSessions()
        {
            SetupSessionBillMiniRedBrownsBayOnJan21From14To15();
            SetupSessionAlbertMiniRedBrownsBayOnJan26From13To14();
            SetupSessionBillMiniRedOrakeiOnJan25From18To19();
            SetupSessionAlbertMiniRedBrownsBayOnJan20From9To10();
            SetupSessionAlbertMiniRedOrakeiOnJan23From11To12();
        }

        protected void SetupCourses()
        {
        }

        protected void SetupCustomers()
        {
        }

        private void SetupBrownsBayLocation()
        {
            var data = new LocationData {Id = new Guid(LOCATION_BROWNS_BAY_ID), Name = "Browns Bay Racquets Club"};

            BusinessRepository.AddLocation(new Guid(BUSINESS_ID), new Location(data));
        }

        protected void SetupOrakeiLocation()
        {
            var data = new LocationData { Id = new Guid(LOCATION_ORAKEI_ID), Name = "Orakei Tennis Club" };

            BusinessRepository.AddLocation(new Guid(BUSINESS_ID), new Location(data));
        }

        protected void SetupCoachAlbert()
        {
            var data = new CoachData
            {
                Id = new Guid(COACH_ALBERT_ID),
                FirstName = "Albert",
                LastName = "Einstein",
                Email = "albert@relativity.net",
                Phone = "299784",
                WorkingHours = SetupStandardWorkingHoursData()
            };

            BusinessRepository.AddCoach(new Guid(BUSINESS_ID), new Coach(data));
        }

        protected void SetupCoachBill()
        {
            var data = new CoachData
            {
                Id = new Guid(COACH_BILL_ID),
                FirstName = "Bill",
                LastName = "Gates",
                Email = "bill@microsoft.com",
                Phone = "095286912",
                WorkingHours = SetupWeekendWorkingHoursData()
            };

            BusinessRepository.AddCoach(new Guid(BUSINESS_ID), new Coach(data));
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

        private void SetupServiceMiniRed()
        {
            var data = new ServiceData
            {
                Id = new Guid(SERVICE_MINI_RED_ID),
                Name = "Mini Red",
                Repetition = new RepetitionData { SessionCount = 1 }
            };

            BusinessRepository.AddService(new Guid(BUSINESS_ID), new Service(data));
        }

        private void SetupServiceMiniOrange()
        {
            var data = new ServiceData
            {
                Id = new Guid(SERVICE_MINI_ORANGE_ID),
                Name = "Mini Orange",
                Repetition = new RepetitionData { SessionCount = 1 }
            };

            BusinessRepository.AddService(new Guid(BUSINESS_ID), new Service(data));
        }

        private CoreData GetCoreData(SessionData sessionData)
        {
            return new CoreData
            {
                Location = BusinessRepository.GetLocation(new Guid(BUSINESS_ID), sessionData.Location.Id),
                Coach = BusinessRepository.GetCoach(new Guid(BUSINESS_ID), sessionData.Coach.Id),
                Service = BusinessRepository.GetService(new Guid(BUSINESS_ID), sessionData.Service.Id)
            };
        }

        private void SetupSessionAlbertMiniRedBrownsBayOnJan20From9To10()
        {
            var data = new SingleSessionData
            {
                Id = new Guid(SESSION_ONE),
                Location = new LocationKeyData {Id = new Guid(LOCATION_BROWNS_BAY_ID)},
                Coach = new CoachKeyData {Id = new Guid(COACH_ALBERT_ID)},
                Service = new ServiceKeyData {Id = new Guid(SERVICE_MINI_RED_ID)},
                Timing = new SessionTimingData("2015-01-20", "9:00", 60),
                Booking = new SessionBookingData(8, true),
                Presentation = new PresentationData {Colour = "Red"},
                Pricing = new SingleSessionPricingData(20)
            };

            BusinessRepository.AddSession(new Guid(BUSINESS_ID), new StandaloneSession(data, GetCoreData(data)));
        }

        private void SetupSessionBillMiniRedBrownsBayOnJan21From14To15()
        {
            var data = new SingleSessionData
            {
                Id = new Guid(SESSION_TWO),
                Location = new LocationKeyData { Id = new Guid(LOCATION_BROWNS_BAY_ID) },
                Coach = new CoachKeyData { Id = new Guid(COACH_BILL_ID) },
                Service = new ServiceKeyData { Id = new Guid(SERVICE_MINI_RED_ID) },
                Timing = new SessionTimingData("2015-01-21", "14:00", 60),
                Booking = new SessionBookingData(8, true),
                Presentation = new PresentationData { Colour = "Red" },
                Pricing = new SingleSessionPricingData(20)
            };

            BusinessRepository.AddSession(new Guid(BUSINESS_ID), new StandaloneSession(data, GetCoreData(data)));
        }

        private void SetupSessionAlbertMiniRedOrakeiOnJan23From11To12()
        {
            var data = new SingleSessionData
            {
                Id = new Guid(SESSION_THREE),
                Location = new LocationKeyData { Id = new Guid(LOCATION_ORAKEI_ID) },
                Coach = new CoachKeyData { Id = new Guid(COACH_ALBERT_ID) },
                Service = new ServiceKeyData { Id = new Guid(SERVICE_MINI_RED_ID) },
                Timing = new SessionTimingData("2015-01-23", "11:00", 60),
                Booking = new SessionBookingData(8, true),
                Presentation = new PresentationData { Colour = "Red" },
                Pricing = new SingleSessionPricingData(20)
            };

            BusinessRepository.AddSession(new Guid(BUSINESS_ID), new StandaloneSession(data, GetCoreData(data)));
        }

        private void SetupSessionBillMiniRedOrakeiOnJan25From18To19()
        {
            var data = new SingleSessionData
            {
                Id = new Guid(SESSION_FOUR),
                Location = new LocationKeyData { Id = new Guid(LOCATION_ORAKEI_ID) },
                Coach = new CoachKeyData { Id = new Guid(COACH_BILL_ID) },
                Service = new ServiceKeyData { Id = new Guid(SERVICE_MINI_RED_ID) },
                Timing = new SessionTimingData("2015-01-25", "18:00", 60),
                Booking = new SessionBookingData(8, true),
                Presentation = new PresentationData { Colour = "Red" },
                Pricing = new SingleSessionPricingData(20)
            };

            BusinessRepository.AddSession(new Guid(BUSINESS_ID), new StandaloneSession(data, GetCoreData(data)));
        }

        private void SetupSessionAlbertMiniRedBrownsBayOnJan26From13To14()
        {
            var data = new SingleSessionData
            {
                Id = new Guid(SESSION_FIVE),
                Location = new LocationKeyData { Id = new Guid(LOCATION_BROWNS_BAY_ID) },
                Coach = new CoachKeyData { Id = new Guid(COACH_ALBERT_ID) },
                Service = new ServiceKeyData { Id = new Guid(SERVICE_MINI_RED_ID) },
                Timing = new SessionTimingData("2015-01-26", "13:00", 60),
                Booking = new SessionBookingData(8, true),
                Presentation = new PresentationData { Colour = "Red" },
                Pricing = new SingleSessionPricingData(20)
            };

            BusinessRepository.AddSession(new Guid(BUSINESS_ID), new StandaloneSession(data, GetCoreData(data)));
        }

        protected void AssertSingleError(Response response, 
                                         string expectedMessage, 
                                         string expectedField = null)
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
