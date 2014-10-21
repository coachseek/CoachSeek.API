using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using NUnit.Framework;
using System;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class LocationAddUseCaseTests : LocationUseCaseTests
    {
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
        public void GivenNoLocationAddCommand_WhenAddLocation_ThenLocationAddFailsWithMissingLocationError()
        {
            var request = GivenNoLocationAddCommand();
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
        public void GivenAUniqueLocation_WhenAddLocation_ThenLocationAddSucceeds()
        {
            var request = GivenAUniqueLocation();
            var response = WhenAddLocation(request);
            ThenLocationAddSucceeds(response);
        }

        private LocationAddCommand GivenNoLocationAddCommand()
        {
            return null;
        }

        private LocationAddCommand GivenNonExistentBusiness()
        {
            return new LocationAddCommand
            {
                BusinessId = new Guid(INVALID_BUSINESS_ID)
            };
        }

        private LocationAddCommand GivenExistingLocation()
        {
            return new LocationAddCommand
            {
                BusinessId = new Guid(BUSINESS_ID),
                LocationName = "  oraKei Tennis Club  "
            };
        }

        private LocationAddCommand GivenAUniqueLocation()
        {
            return new LocationAddCommand
            {
                BusinessId = new Guid(BUSINESS_ID),
                LocationName = "Mt Roskill Squash Club"
            };
        }

        private Response<LocationData> WhenAddLocation(LocationAddCommand command)
        {
            var useCase = new LocationAddUseCase(BusinessRepository);

            return useCase.AddLocation(command);
        }

        private void ThenLocationAddFailsWithMissingLocationError(Response<LocationData> response)
        {
            AssertMissingLocationError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenLocationAddFailsWithInvalidBusinessError(Response<LocationData> response)
        {
            AssertInvalidBusinessError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenLocationAddFailsWithDuplicateLocationError(Response<LocationData> response)
        {
            AssertDuplicateLocationError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenLocationAddSucceeds(Response<LocationData> response)
        {
            AssertSaveBusinessIsCalled();
            AssertResponseReturnsNewLocation(response);
        }

        private void AssertMissingLocationError(Response<LocationData> response)
        {
            AssertSingleError(response, "Missing location data.");
        }

        private void AssertInvalidBusinessError(Response<LocationData> response)
        {
            AssertSingleError(response, "This business does not exist.");
        }

        private void AssertDuplicateLocationError(Response<LocationData> response)
        {
            AssertSingleError(response, "This location already exists.");
        }

        private void AssertResponseReturnsNewLocation(Response<LocationData> response)
        {
            var location = response.Data;
            Assert.That(location, Is.Not.Null);
            Assert.That(location.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(location.Name, Is.EqualTo("Mt Roskill Squash Club"));
        }
    }
}
