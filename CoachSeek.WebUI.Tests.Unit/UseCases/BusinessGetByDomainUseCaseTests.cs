using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.UseCases.Responses;
using CoachSeek.WebUI.Persistence;
using CoachSeek.WebUI.UseCases;
using NUnit.Framework;

namespace CoachSeek.WebUI.Tests.Unit.UseCases
{
    [TestFixture]
    public class BusinessGetByDomainUseCaseTests
    {
        private const string INVALID_BUSINESS_ID = "A031E489-C1D4-4889-A3E0-196930BA35DD";
        private const string VALID_BUSINESS_ID = "87738006-7E54-4FA6-9CF5-7FEAB5940668";
        private const string LOCATION_ID = "BE94064D-7033-4CF8-9F47-7E118A393C2E";
        private const string COACH_ID = "65FF663E-C858-444B-800D-268D61F17E43";

        private InMemoryBusinessRepository BusinessRepository { get; set; }

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
            var business = new Business(SetupLocations(), SetupCoaches())
            {
                Id = new Guid(VALID_BUSINESS_ID),
                Name = "Olaf's Bookshoppe",
                Domain = "olafsbookshoppe",
                Admin = new BusinessAdmin("Olaf", "Thielke", "olaft@ihug.co.nz", "Password1")
            };

            return business;
        }

        private IEnumerable<Location> SetupLocations()
        {
            return new List<Location>
            {
                new Location {Id = new Guid(LOCATION_ID), Name = "HQ"}
            };
        }

        private IEnumerable<Coach> SetupCoaches()
        {
            return new List<Coach>
            {
                new Coach(new Guid(COACH_ID), "Bob", "Smith", "bob.smith@example.com", "021987654")
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


        private BusinessGetResponse WhenGetBusinessByDomain(string domain)
        {
            var useCase = new BusinessGetByDomainUseCase(BusinessRepository);

            return useCase.GetByDomain(domain);
        }


        private void ThenBusinessGetFailsWithMissingDomainError(BusinessGetResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1040));
            Assert.That(error.Message, Is.EqualTo("Missing domain."));
            Assert.That(error.Field, Is.Null);
        }

        private void ThenBusinessGetReturnsNoBusiness(BusinessGetResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Null);
        }
    }
}
