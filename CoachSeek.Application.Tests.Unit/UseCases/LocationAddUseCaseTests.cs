using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Repositories;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.Application.Tests.Unit.UseCases
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
            BusinessRepository.Clear();

            var business = SetupOlafsCafeBusiness();
            BusinessRepository.Add(business);

            BusinessRepository.WasSaveNewBusinessCalled = false;
            BusinessRepository.WasSaveBusinessCalled = false;
        }

        private Business SetupOlafsCafeBusiness()
        {
            return new Business(new Guid(VALID_BUSINESS_ID),
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
                new List<LocationData> { new LocationData { Id = new Guid(LOCATION_ID), Name = "HQ" }},
                new List<CoachData>());
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
                BusinessId = new Guid(VALID_BUSINESS_ID),
                LocationName = "  hq"
            };
        }

        private LocationAddCommand GivenAUniqueLocation()
        {
            return new LocationAddCommand
            {
                BusinessId = new Guid(VALID_BUSINESS_ID),
                LocationName = "Discount Store  "
            };
        }

        private LocationAddResponse WhenAddLocation(LocationAddCommand command)
        {
            var useCase = new LocationAddUseCase(BusinessRepository);

            return useCase.AddLocation(command);
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

        private void ThenLocationAddSucceeds(LocationAddResponse response)
        {
            AssertSaveBusinessIsCalled();
            AssertSavedBusinessWithTwoLocations(response);
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

        private void AssertSavedBusinessWithTwoLocations(LocationAddResponse response)
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
