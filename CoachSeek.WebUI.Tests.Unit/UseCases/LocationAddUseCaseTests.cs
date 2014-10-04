using System.Collections.Generic;
using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.Requests;
using CoachSeek.WebUI.Models.Responses;
using CoachSeek.WebUI.Persistence;
using CoachSeek.WebUI.UseCases;
using NUnit.Framework;
using System;
using System.Linq;

namespace CoachSeek.WebUI.Tests.Unit.UseCases
{
    [TestFixture]
    public class LocationAddUseCaseTests
    {
        private const string INVALID_BUSINESS_ID = "A031E489-C1D4-4889-A3E0-196930BA35DD";
        private const string VALID_BUSINESS_ID = "87738006-7E54-4FA6-9CF5-7FEAB5940668";
        private const string LOCATION_ID = "BE94064D-7033-4CF8-9F47-7E118A393C2E";

        private InMemoryBusinessRepository BusinessRepository { get; set; }

        [SetUp]
        public void Setup()
        {
            SetupBusinessRepository();
        }

        private void SetupBusinessRepository()
        {
            BusinessRepository = new InMemoryBusinessRepository();
            var business = SetupOlafsCafeBusiness();
            BusinessRepository.Add(business);

            BusinessRepository.WasSaveNewBusinessCalled = false;
            BusinessRepository.WasSaveBusinessCalled = false;
        }

        private Business SetupOlafsCafeBusiness()
        {
            var location = new Location { Id = new Guid(LOCATION_ID), Name = "HQ" };

            var business = new Business(new List<Location> { location })
            {
                Id = new Guid(VALID_BUSINESS_ID),
                Name = "Olaf's Bookshoppe",
                Domain = "olafsbookshoppe",
                Admin = new BusinessAdmin
                {
                    FirstName = "Olaf",
                    LastName = "Thielke",
                    Email = "olaft@ihug.co.nz",
                    Password = "abc123"
                },
            };

            return business;
        }

        [Test]
        public void GivenNoLocationAddRequest_WhenAddLocation_ThenLocationAddFailsWithMissingLocationError()
        {
            var request = GivenNoLocationAddRequest();
            var response = WhenAddLocation(request);
            ThenLocationAddFailsWithMissingLocationError(response);
        }

        [Test]
        public void GivenNonExistentBusiness_WhenAddLocation_ThenLocationAddFailsWithInvalidBusinessError()
        {
            var request = GivenNonExistentBusiness();
            var response = WhenAddLocation(request);
            ThenLocationAddFailsWithInvalidBusinessError(response);
        }

        [Test]
        public void GivenExistingLocation_WhenAddLocation_ThenLocationAddFailsWithDuplicateLocationError()
        {
            var request = GivenExistingLocation();
            var response = WhenAddLocation(request);
            ThenLocationAddFailsWithDuplicateLocationError(response);
        }

        [Test]
        public void GivenAUniqueLocation_WhenAddLocation_ThenTheLocationAddSucceeds()
        {
            var request = GivenAUniqueLocation();
            var response = WhenAddLocation(request);
            ThenTheLocationAddSucceeds(response);
        }

        private LocationAddRequest GivenNoLocationAddRequest()
        {
            return null;
        }

        private LocationAddRequest GivenNonExistentBusiness()
        {
            return new LocationAddRequest
            {
                BusinessId = new Guid(INVALID_BUSINESS_ID)
            };
        }

        private LocationAddRequest GivenExistingLocation()
        {
            return new LocationAddRequest
            {
                BusinessId = new Guid(VALID_BUSINESS_ID),
                LocationName = "hq"
            };
        }

        private LocationAddRequest GivenAUniqueLocation()
        {
            return new LocationAddRequest
            {
                BusinessId = new Guid(VALID_BUSINESS_ID),
                LocationName = "Discount Store"
            };
        }

        private LocationAddResponse WhenAddLocation(LocationAddRequest request)
        {
            var useCase = new LocationAddUseCase(BusinessRepository);

            return useCase.AddLocation(request);
        }

        private void ThenLocationAddFailsWithMissingLocationError(LocationAddResponse response)
        {
            AssertMissingLocationError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenLocationAddFailsWithInvalidBusinessError(LocationAddResponse response)
        {
            AssertInvalidBusinessError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenLocationAddFailsWithDuplicateLocationError(LocationAddResponse response)
        {
            AssertDuplicateLocationError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenTheLocationAddSucceeds(LocationAddResponse response)
        {
            AssertSaveBusinessIsCalled();
            AssertSaveBusinessWithTwoLocations(response);
        }

        private void AssertMissingLocationError(LocationAddResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1110));
            Assert.That(error.Message, Is.EqualTo("Missing location data."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertInvalidBusinessError(LocationAddResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1030));
            Assert.That(error.Message, Is.EqualTo("This business does not exist."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertDuplicateLocationError(LocationAddResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1120));
            Assert.That(error.Message, Is.EqualTo("This location already exists."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertSaveBusinessIsNotCalled()
        {
            Assert.That(BusinessRepository.WasSaveBusinessCalled, Is.False);
        }

        private void AssertSaveBusinessIsCalled()
        {
            Assert.That(BusinessRepository.WasSaveBusinessCalled, Is.True);
        }

        private void AssertSaveBusinessWithTwoLocations(LocationAddResponse response)
        {
            Assert.That(response.Business, Is.Not.Null);
            Assert.That(response.Business.Locations.Count, Is.EqualTo(2));

            var firstLocation = response.Business.Locations[0];
            Assert.That(firstLocation.Id, Is.EqualTo(new Guid(LOCATION_ID)));
            Assert.That(firstLocation.Name, Is.EqualTo("HQ"));

            var secondLocation = response.Business.Locations[1];
            Assert.That(secondLocation.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(secondLocation.Name, Is.EqualTo("Discount Store"));
        }
    }
}
