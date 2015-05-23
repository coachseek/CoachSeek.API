using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using NUnit.Framework;
using System;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class 
        
        LocationAddUseCaseTests : LocationUseCaseTests
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
            ThenLocationAddSucceeds();
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
            var useCase = new LocationAddUseCase();
            var context = new ApplicationContext(new BusinessContext(new Guid(BUSINESS_ID), "", BusinessRepository), null, true);
            useCase.Initialise(context);
            return useCase.AddLocation(command);
        }


        private void ThenLocationAddFailsWithMissingLocationError(Response response)
        {
            AssertMissingLocationError(response);

            Assert.That(BusinessRepository.WasAddLocationCalled, Is.False);
        }

        private void ThenLocationAddFailsWithDuplicateLocationError(Response response)
        {
            AssertDuplicateLocationError(response);

            Assert.That(BusinessRepository.WasAddLocationCalled, Is.False);
        }

        private void ThenLocationAddSucceeds()
        {
            Assert.That(BusinessRepository.WasAddLocationCalled, Is.True);

            Assert.That(BusinessRepository.BusinessIdPassedIn, Is.EqualTo(new Guid(BUSINESS_ID)));

            var location = (Location)BusinessRepository.DataPassedIn;
            Assert.That(location.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(location.Name, Is.EqualTo("Mt Roskill Squash Club"));
        }

        private void AssertMissingLocationError(Response response)
        {
            AssertSingleError(response, "Missing data.");
        }

        private void AssertDuplicateLocationError(Response response)
        {
            AssertSingleError(response, "This location already exists.", "location.name");
        }
    }
}
