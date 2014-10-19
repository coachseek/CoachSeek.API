using CoachSeek.Application.Contracts.Models.Responses;
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
        public void GivenNonExistentBusiness_WhenUpdateLocation_ThenLocationUpdateFailsWithInvalidBusinessError()
        {
            var request = GivenNonExistentBusiness();
            var response = WhenUpdateLocation(request);
            ThenLocationUpdateFailsWithInvalidBusinessError(response);
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
                LocationId = new Guid(LOCATION_REMUERA_ID),
                LocationName = "Remuera Tennis Club"
            };
        }

        private LocationUpdateCommand GivenExistingLocationName()
        {
            // Note: We are intentionally using Orakei's Id and Browns Bay's Name.
            return new LocationUpdateCommand
            {
                BusinessId = new Guid(BUSINESS_ID),
                LocationId = new Guid(LOCATION_ORAKEI_ID),
                LocationName = "  Browns Bay Racquets Club   "
            };
        }

        private LocationUpdateCommand GivenAUniqueLocationName()
        {
            return new LocationUpdateCommand
            {
                BusinessId = new Guid(BUSINESS_ID),
                LocationId = new Guid(LOCATION_BROWNS_BAY_ID),
                LocationName = "  Browns Bay Tennis & Squash Club  "
            };
        }

        private Response<LocationData> WhenUpdateLocation(LocationUpdateCommand request)
        {
            var useCase = new LocationUpdateUseCase(BusinessRepository);

            return useCase.UpdateLocation(request);
        }

        private void ThenLocationUpdateFailsWithMissingLocationError(Response<LocationData> response)
        {
            AssertMissingLocationError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenLocationUpdateFailsWithInvalidBusinessError(Response<LocationData> response)
        {
            AssertInvalidBusinessError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenLocationUpdateFailsWithInvalidLocationError(Response<LocationData> response)
        {
            AssertInvalidLocationError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenLocationUpdateFailsWithDuplicateLocationError(Response<LocationData> response)
        {
            AssertDuplicateLocationError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenLocationUpdateSucceeds(Response<LocationData> response)
        {
            AssertSaveBusinessIsCalled();
            AssertResponseReturnsUpdatedLocation(response);
        }

        private void AssertMissingLocationError(Response<LocationData> response)
        {
            AssertSingleError(response, 1130, "Missing location data.");
        }

        private void AssertInvalidBusinessError(Response<LocationData> response)
        {
            AssertSingleError(response, 1030, "This business does not exist.");
        }

        private void AssertInvalidLocationError(Response<LocationData> response)
        {
            AssertSingleError(response, 1140, "This location does not exist.");
        }

        private void AssertDuplicateLocationError(Response<LocationData> response)
        {
            AssertSingleError(response, 1120, "This location already exists.");
        }

        private void AssertResponseReturnsUpdatedLocation(Response<LocationData> response)
        {
            var location = response.Data;
            Assert.That(location, Is.Not.Null);
            Assert.That(location.Id, Is.EqualTo(new Guid(LOCATION_BROWNS_BAY_ID)));
            Assert.That(location.Name, Is.EqualTo("Browns Bay Tennis & Squash Club"));
        }
    }
}
