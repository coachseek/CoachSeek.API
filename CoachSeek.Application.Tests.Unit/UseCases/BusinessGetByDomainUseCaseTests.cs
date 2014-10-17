using CoachSeek.Application.Configuration;
using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.UseCases;
using CoachSeek.Data.Model;
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

        private InMemoryBusinessRepository BusinessRepository { get; set; }

        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            ApplicationAutoMapperConfigurator.Configure();
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

            var business = SetupOlafsCafeBusiness();
            BusinessRepository.Add(business);

            BusinessRepository.WasSaveNewBusinessCalled = false;
            BusinessRepository.WasSaveBusinessCalled = false;
        }

        private Business SetupOlafsCafeBusiness()
        {
            return new Business(
                new Guid(VALID_BUSINESS_ID),
                "Olaf's Bookshoppe",
                "olafsbookshoppe",
                new BusinessAdminData
                {
                    FirstName = "Olaf",
                    LastName = "Thielke",
                    Email = "olaft@ihug.co.nz",
                    Username = "olaft@ihug.co.nz",
                    PasswordHash = "Password1"
                },
                SetupLocations(),
                SetupCoaches()
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
                    Phone = "021987654"
                }, 
            };
        }


        [Test]
        public void GivenNullDomainName_WhenGetBusinessByDomain_ThenBusinessGetFailsWithMissingDomainError()
        {
            var domain = GivenNullDomainName();
            var response = WhenGetBusinessByDomain(domain);
            ThenBusinessGetFailsWithMissingDomainError(response);
        }

        [Test]
        public void GivenEmptyDomainName_WhenGetBusinessByDomain_ThenBusinessGetFailsWithMissingDomainError()
        {
            var domain = GivenEmptyDomainName();
            var response = WhenGetBusinessByDomain(domain);
            ThenBusinessGetFailsWithMissingDomainError(response);
        }

        [Test]
        public void GivenWhitespaceDomainName_WhenGetBusinessByDomain_ThenBusinessGetFailsWithMissingDomainError()
        {
            var domain = GivenWhitespaceDomainName();
            var response = WhenGetBusinessByDomain(domain);
            ThenBusinessGetFailsWithMissingDomainError(response);
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


        private Response WhenGetBusinessByDomain(string domain)
        {
            var useCase = new BusinessGetByDomainUseCase(BusinessRepository);

            return useCase.GetByDomain(domain);
        }


        private void ThenBusinessGetFailsWithMissingDomainError(Response response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1040));
            Assert.That(error.Message, Is.EqualTo("Missing domain."));
            Assert.That(error.Field, Is.Null);
        }

        private void ThenBusinessGetReturnsNoBusiness(Response response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Null);
        }
    }
}
