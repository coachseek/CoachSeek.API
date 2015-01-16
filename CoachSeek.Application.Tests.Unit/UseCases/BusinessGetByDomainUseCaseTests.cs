using CoachSeek.Application.Configuration;
using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Configuration;
using CoachSeek.DataAccess.Repositories;
using CoachSeek.Domain.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class BusinessGetByDomainUseCaseTests
    {
        private const string INVALID_BUSINESS_ID = "A031E489-C1D4-4889-A3E0-196930BA35DD";
        private const string VALID_BUSINESS_ID = "87738006-7E54-4FA6-9CF5-7FEAB5940668";
        private const string LOCATION_ID = "BE94064D-7033-4CF8-9F47-7E118A393C2E";
        private const string COACH_ID = "65FF663E-C858-444B-800D-268D61F17E43";
        private const string SERVICE_ID = "FBE6C397-6D3E-48B6-A2DF-1BD685C034AE";

        private InMemoryBusinessRepository BusinessRepository { get; set; }

        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            ApplicationAutoMapperConfigurator.Configure();
            DbAutoMapperConfigurator.Configure();
        }

        [SetUp]
        public void Setup()
        {
            SetupBusinessRepository();
        }

        private void SetupBusinessRepository()
        {
            BusinessRepository = new InMemoryBusinessRepository();
            BusinessRepository.Clear();

            var business = SetupOlafsCoachingBusiness();
            BusinessRepository.Add(business);

            BusinessRepository.WasSaveNewBusinessCalled = false;
            BusinessRepository.WasSaveBusinessCalled = false;
        }

        private Business SetupOlafsCoachingBusiness()
        {
            return new Business(
                new Guid(VALID_BUSINESS_ID),
                "Olaf's Coaching",
                "olafscoaching",
                SetupLocations(),
                SetupCoaches(),
                SetupServices(),
                null
            );
        }

        private IEnumerable<LocationData> SetupLocations()
        {
            return new List<LocationData>
            {
                new LocationData {Id = new Guid(LOCATION_ID), Name = "HQ"}
            };
        }

        private IEnumerable<CoachData> SetupCoaches()
        {
            return new List<CoachData>
            {
                new CoachData
                {
                    Id = new Guid(COACH_ID),
                    FirstName = "Bob", 
                    LastName = "Smith", 
                    Email = "bob.smith@example.com",
                    Phone = "021987654",
                    WorkingHours = SetupStandardWorkingHourDatas()
                }, 
            };
        }

        private IEnumerable<ServiceData> SetupServices()
        {
            return new List<ServiceData>
            {
                new ServiceData
                {
                    Id = new Guid(SERVICE_ID),
                    Name = "Mini Red",
                    Repetition = new RepetitionData { SessionCount = 1 }
                }
            };
        }

        private WeeklyWorkingHoursData SetupStandardWorkingHourDatas()
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


        [Test]
        public void GivenNullDomainName_WhenGetBusinessByDomain_ThenBusinessGetReturnsNoBusiness()
        {
            var domain = GivenNullDomainName();
            var response = WhenGetBusinessByDomain(domain);
            ThenBusinessGetReturnsNoBusiness(response);
        }

        [Test]
        public void GivenEmptyDomainName_WhenGetBusinessByDomain_ThenBusinessGetReturnsNoBusiness()
        {
            var domain = GivenEmptyDomainName();
            var response = WhenGetBusinessByDomain(domain);
            ThenBusinessGetReturnsNoBusiness(response);
        }

        [Test]
        public void GivenWhitespaceDomainName_WhenGetBusinessByDomain_ThenBusinessGetReturnsNoBusiness()
        {
            var domain = GivenWhitespaceDomainName();
            var response = WhenGetBusinessByDomain(domain);
            ThenBusinessGetReturnsNoBusiness(response);
        }

        [Test]
        public void GivenDomainForNonExistentBusiness_WhenGetBusinessByDomain_ThenBusinessGetReturnsNoBusiness()
        {
            var request = GivenDomainForNonExistentBusiness();
            var response = WhenGetBusinessByDomain(request);
            ThenBusinessGetReturnsNoBusiness(response);
        }


        private string GivenNullDomainName()
        {
            return null;
        }

        private string GivenEmptyDomainName()
        {
            return "";
        }

        private string GivenWhitespaceDomainName()
        {
            return "    ";
        }

        private string GivenDomainForNonExistentBusiness()
        {
            return "highstreethookers";
        }


        private BusinessData WhenGetBusinessByDomain(string domain)
        {
            var useCase = new BusinessGetByDomainUseCase(BusinessRepository);

            return useCase.GetByDomain(domain);
        }


        private void ThenBusinessGetFailsWithMissingDomainError(Response<BusinessData> response)
        {
            Assert.That(response.Data, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Message, Is.EqualTo("Missing domain."));
            Assert.That(error.Field, Is.Null);
        }

        private void ThenBusinessGetReturnsNoBusiness(BusinessData response)
        {
            Assert.That(response, Is.Null);
        }
    }
}
