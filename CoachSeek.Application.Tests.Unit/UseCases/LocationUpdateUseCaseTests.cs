using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.UseCases;
using CoachSeek.Common;
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
            SetupUserRepository();
        }


        [Test]
        public void GivenNonExistentLocation_WhenUpdateLocation_ThenLocationUpdateFailsWithInvalidLocationError()
        {
            var command = GivenNonExistentLocation();
            var response = WhenUpdateLocation(command);
            ThenLocationUpdateFailsWithInvalidLocationError(response, command.Id);
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

        private LocationUpdateCommand GivenNonExistentLocation()
        {
            return new LocationUpdateCommand
            {
                Id = Guid.NewGuid(),
                Name = "Remuera Tennis Club"
            };
        }

        private LocationUpdateCommand GivenExistingLocationName()
        {
            // Note: We are intentionally using Orakei's Id and Browns Bay's Name.
            return new LocationUpdateCommand
            {
                Id = new Guid(LOCATION_ORAKEI_ID),
                Name = "  Browns Bay Racquets Club   "
            };
        }

        private LocationUpdateCommand GivenAUniqueLocationName()
        {
            return new LocationUpdateCommand
            {
                Id = new Guid(LOCATION_BROWNS_BAY_ID),
                Name = "  Browns Bay Tennis & Squash Club  "
            };
        }

        private IResponse WhenUpdateLocation(LocationUpdateCommand request)
        {
            var useCase = new LocationUpdateUseCase();
            var business = new BusinessDetails(new Guid(BUSINESS_ID), "", "", DateTime.UtcNow.AddDays(1));
            var currency = new CurrencyDetails("NZD", "$");
            var businessContext = new BusinessContext(business, currency, BusinessRepository, null, UserRepository);
            var emailContext = new EmailContext(true, false, "", null);
            var context = new ApplicationContext(null, businessContext, emailContext, null, true);
            useCase.Initialise(context);
            return useCase.UpdateLocation(request);
        }

        private void ThenLocationUpdateFailsWithMissingLocationError(Response response)
        {
            AssertMissingLocationError(response);

            Assert.That(BusinessRepository.WasUpdateLocationCalled, Is.False);
        }

        private void ThenLocationUpdateFailsWithInvalidLocationError(IResponse response, Guid locationId)
        {
            AssertInvalidLocationError(response, locationId);

            Assert.That(BusinessRepository.WasUpdateLocationCalled, Is.False);
        }

        private void ThenLocationUpdateFailsWithDuplicateLocationError(IResponse response)
        {
            AssertDuplicateLocationError(response);

            Assert.That(BusinessRepository.WasUpdateLocationCalled, Is.False);
        }

        private void ThenLocationUpdateSucceeds(IResponse response)
        {
            Assert.That(BusinessRepository.WasUpdateLocationCalled, Is.True);

            AssertResponseReturnsUpdatedLocation(response);
        }

        private void AssertMissingLocationError(IResponse response)
        {
            AssertSingleError(response, ErrorCodes.DataRequired, "Missing data.");
        }

        private void AssertInvalidLocationError(IResponse response, Guid locationId)
        {
            AssertSingleError(response, ErrorCodes.LocationInvalid, "This location does not exist.", locationId.ToString());
        }

        private void AssertDuplicateLocationError(IResponse response)
        {
            AssertSingleError(response, ErrorCodes.LocationDuplicate, "Location 'Browns Bay Racquets Club' already exists.", "Browns Bay Racquets Club");
        }

        private void AssertResponseReturnsUpdatedLocation(IResponse response)
        {
            var location = (LocationData)response.Data;
            Assert.That(location, Is.Not.Null);
            Assert.That(location.Id, Is.EqualTo(new Guid(LOCATION_BROWNS_BAY_ID)));
            Assert.That(location.Name, Is.EqualTo("Browns Bay Tennis & Squash Club"));
        }
    }
}
