﻿using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using NUnit.Framework;
using System;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class LocationUpdateUseCaseTests : LocationUseCaseTests
    {
        private const string LOCATION_REMUERA_ID = "0574C8C4-6172-45F8-847D-C14EE362A3AD";

        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            ConfigureAutoMapper();
        }

        [SetUp]
        public void Setup()
        {
            SetupBusinessRepository();
        }

        
        [Test]
        public void GivenNoLocationUpdateRequest_WhenUpdateLocation_ThenLocationUpdateFailsWithMissingLocationError()
        {
            var request = GivenNoLocationUpdateRequest();
            var response = WhenUpdateLocation(request);
            ThenLocationUpdateFailsWithMissingLocationError(response);
        }

        [Test]
        public void GivenNonExistentLocation_WhenUpdateLocation_ThenLocationUpdateFailsWithInvalidLocationError()
        {
            var request = GivenNonExistentLocation();
            var response = WhenUpdateLocation(request);
            ThenLocationUpdateFailsWithInvalidLocationError(response);
        }

        [Test]
        public void GivenExistingLocationName_WhenUpdateLocation_ThenLocationUpdateFailsWithDuplicateLocationError()
        {
            var request = GivenExistingLocationName();
            var response = WhenUpdateLocation(request);
            ThenLocationUpdateFailsWithDuplicateLocationError(response);
        }

        [Test]
        public void GivenAUniqueLocationName_WhenUpdateLocation_ThenLocationUpdateSucceeds()
        {
            var request = GivenAUniqueLocationName();
            var response = WhenUpdateLocation(request);
            ThenLocationUpdateSucceeds(response);
        }

        private LocationUpdateCommand GivenNoLocationUpdateRequest()
        {
            return null;
        }

        private LocationUpdateCommand GivenNonExistentBusiness()
        {
            return new LocationUpdateCommand
            {
                BusinessId = new Guid(INVALID_BUSINESS_ID)
            };
        }

        private LocationUpdateCommand GivenNonExistentLocation()
        {
            return new LocationUpdateCommand
            {
                BusinessId = new Guid(BUSINESS_ID),
                Id = new Guid(LOCATION_REMUERA_ID),
                Name = "Remuera Tennis Club"
            };
        }

        private LocationUpdateCommand GivenExistingLocationName()
        {
            // Note: We are intentionally using Orakei's Id and Browns Bay's Name.
            return new LocationUpdateCommand
            {
                BusinessId = new Guid(BUSINESS_ID),
                Id = new Guid(LOCATION_ORAKEI_ID),
                Name = "  Browns Bay Racquets Club   "
            };
        }

        private LocationUpdateCommand GivenAUniqueLocationName()
        {
            return new LocationUpdateCommand
            {
                BusinessId = new Guid(BUSINESS_ID),
                Id = new Guid(LOCATION_BROWNS_BAY_ID),
                Name = "  Browns Bay Tennis & Squash Club  "
            };
        }

        private Response WhenUpdateLocation(LocationUpdateCommand request)
        {
            var useCase = new LocationUpdateUseCase(BusinessRepository);

            return useCase.UpdateLocation(request);
        }

        private void ThenLocationUpdateFailsWithMissingLocationError(Response response)
        {
            AssertMissingLocationError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenLocationUpdateFailsWithInvalidBusinessError(Response response)
        {
            AssertInvalidBusinessError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenLocationUpdateFailsWithInvalidLocationError(Response response)
        {
            AssertInvalidLocationError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenLocationUpdateFailsWithDuplicateLocationError(Response response)
        {
            AssertDuplicateLocationError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenLocationUpdateSucceeds(Response response)
        {
            AssertSaveBusinessIsCalled();
            AssertResponseReturnsUpdatedLocation(response);
        }

        private void AssertMissingLocationError(Response response)
        {
            AssertSingleError(response, "Missing data.");
        }

        private void AssertInvalidBusinessError(Response response)
        {
            AssertSingleError(response, "This business does not exist.", "location.businessId");
        }

        private void AssertInvalidLocationError(Response response)
        {
            AssertSingleError(response, "This location does not exist.", "location.id");
        }

        private void AssertDuplicateLocationError(Response response)
        {
            AssertSingleError(response, "This location already exists.", "location.name");
        }

        private void AssertResponseReturnsUpdatedLocation(Response response)
        {
            var location = (LocationData)response.Data;
            Assert.That(location, Is.Not.Null);
            Assert.That(location.Id, Is.EqualTo(new Guid(LOCATION_BROWNS_BAY_ID)));
            Assert.That(location.Name, Is.EqualTo("Browns Bay Tennis & Squash Club"));
        }
    }
}
