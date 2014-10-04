using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.UseCases.Requests;
using CoachSeek.WebUI.Models.UseCases.Responses;
using CoachSeek.WebUI.Persistence;
using CoachSeek.WebUI.UseCases;
using NUnit.Framework;

namespace CoachSeek.WebUI.Tests.Unit.UseCases
{
    [TestFixture]
    public class LocationUpdateUseCaseTests
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
        public void GivenNoLocationUpdateRequest_WhenUpdateLocation_ThenLocationUpdateFailsWithMissingLocationError()
        {
            var request = GivenNoLocationUpdateRequest();
            var response = WhenUpdateLocation(request);
            ThenLocationUpdateFailsWithMissingLocationError(response);
        }

        [Test]
        public void GivenNonExistentBusiness_WhenUpdateLocation_ThenLocationUpdateFailsWithInvalidBusinessError()
        {
            var request = GivenNonExistentBusiness();
            var response = WhenUpdateLocation(request);
            ThenLocationUpdateFailsWithInvalidBusinessError(response);
        }

        private LocationUpdateRequest GivenNoLocationUpdateRequest()
        {
            return null;
        }

        private LocationUpdateRequest GivenNonExistentBusiness()
        {
            return new LocationUpdateRequest
            {
                BusinessId = new Guid(INVALID_BUSINESS_ID)
            };
        }

        private LocationUpdateResponse WhenUpdateLocation(LocationUpdateRequest request)
        {
            var useCase = new LocationUpdateUseCase(BusinessRepository);

            return useCase.UpdateLocation(request);
        }

        private void ThenLocationUpdateFailsWithMissingLocationError(LocationUpdateResponse response)
        {
            AssertMissingLocationError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenLocationUpdateFailsWithInvalidBusinessError(LocationUpdateResponse response)
        {
            AssertInvalidBusinessError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void AssertMissingLocationError(LocationUpdateResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1130));
            Assert.That(error.Message, Is.EqualTo("Missing location data."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertInvalidBusinessError(LocationUpdateResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1030));
            Assert.That(error.Message, Is.EqualTo("This business does not exist."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertSaveBusinessIsNotCalled()
        {
            Assert.That(BusinessRepository.WasSaveBusinessCalled, Is.False);
        }
    }
}
