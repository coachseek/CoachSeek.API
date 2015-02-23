using CoachSeek.Application.Contracts.Models;
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

        private LocationAddCommand GivenExistingLocation()
        {
            return new LocationAddCommand
            {
                Name = "  oraKei Tennis Club  "
            };
        }

        private LocationAddCommand GivenAUniqueLocation()
        {
            return new LocationAddCommand
            {
                Name = "Mt Roskill Squash Club"
            };
        }

        private Response WhenAddLocation(LocationAddCommand command)
        {
            var useCase = new LocationAddUseCase(BusinessRepository) {BusinessId = new Guid(BUSINESS_ID)};

            return useCase.AddLocation(command);
        }

        private void ThenLocationAddFailsWithMissingLocationError(Response response)
        {
            AssertMissingLocationError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenLocationAddFailsWithDuplicateLocationError(Response response)
        {
            AssertDuplicateLocationError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenLocationAddSucceeds(Response response)
        {
            AssertSaveBusinessIsCalled();
            AssertResponseReturnsNewLocation(response);
        }

        private void AssertMissingLocationError(Response response)
        {
            AssertSingleError(response, "Missing data.");
        }

        private void AssertInvalidBusinessError(Response response)
        {
            AssertSingleError(response, "This business does not exist.", "location.businessId");
        }

        private void AssertDuplicateLocationError(Response response)
        {
            AssertSingleError(response, "This location already exists.", "location.name");
        }

        private void AssertResponseReturnsNewLocation(Response response)
        {
            var location = (LocationData)response.Data;
            Assert.That(location, Is.Not.Null);
            Assert.That(location.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(location.Name, Is.EqualTo("Mt Roskill Squash Club"));
        }
    }
}
