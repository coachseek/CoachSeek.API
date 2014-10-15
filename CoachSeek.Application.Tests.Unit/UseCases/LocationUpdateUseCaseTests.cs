﻿using CoachSeek.Application.Contracts.Models.Responses;
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
    public class LocationUpdateUseCaseTests
    {
        private const string INVALID_BUSINESS_ID = "A031E489-C1D4-4889-A3E0-196930BA35DD";
        private const string VALID_BUSINESS_ID = "87738006-7E54-4FA6-9CF5-7FEAB5940668";
        private const string INVALID_LOCATION_ID = "9D5D225A-F821-41C2-BBE7-84A7B5DA90E5";
        private const string VALID_LOCATION_ID = "BE94064D-7033-4CF8-9F47-7E118A393C2E";


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
                SetupLocations(),
                SetupCoaches());
        }

        private IEnumerable<LocationData> SetupLocations()
        {
            return new List<LocationData>
            {
                new LocationData {Id = new Guid("7D72F6E4-07D1-463F-84E6-865DE53D9A67"), Name = "HQ"},
                new LocationData {Id = new Guid(VALID_LOCATION_ID), Name = "Store"},
            };
        }

        private IEnumerable<CoachData> SetupCoaches()
        {
            return new List<CoachData>();
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
                BusinessId = new Guid(VALID_BUSINESS_ID),
                LocationId = new Guid(INVALID_LOCATION_ID),
                LocationName = "discount store"
            };
        }

        private LocationUpdateCommand GivenExistingLocationName()
        {
            return new LocationUpdateCommand
            {
                BusinessId = new Guid(VALID_BUSINESS_ID),
                LocationId = new Guid(VALID_LOCATION_ID),
                LocationName = "hq"
            };
        }

        private LocationUpdateCommand GivenAUniqueLocationName()
        {
            return new LocationUpdateCommand
            {
                BusinessId = new Guid(VALID_BUSINESS_ID),
                LocationId = new Guid(VALID_LOCATION_ID),
                LocationName = "Queen St Store"
            };
        }

        private LocationUpdateResponse WhenUpdateLocation(LocationUpdateCommand request)
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

        private void ThenLocationUpdateFailsWithInvalidLocationError(LocationUpdateResponse response)
        {
            AssertInvalidLocationError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenLocationUpdateFailsWithDuplicateLocationError(LocationUpdateResponse response)
        {
            AssertDuplicateLocationError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenLocationUpdateSucceeds(LocationUpdateResponse response)
        {
            AssertSaveBusinessIsCalled();
            AssertLocationIsUpdated(response);
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

        private void AssertInvalidLocationError(LocationUpdateResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1140));
            Assert.That(error.Message, Is.EqualTo("This location does not exist."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertDuplicateLocationError(LocationUpdateResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1120));
            Assert.That(error.Message, Is.EqualTo("This location already exists."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertLocationIsUpdated(LocationUpdateResponse response)
        {
            Assert.That(response.Business, Is.Not.Null);
            Assert.That(response.Business.Locations.Count, Is.EqualTo(2));

            var firstLocation = response.Business.Locations[0];
            Assert.That(firstLocation.Id, Is.EqualTo(new Guid("7D72F6E4-07D1-463F-84E6-865DE53D9A67")));
            Assert.That(firstLocation.Name, Is.EqualTo("HQ"));

            var secondLocation = response.Business.Locations[1];
            Assert.That(secondLocation.Id, Is.EqualTo(new Guid(VALID_LOCATION_ID)));
            Assert.That(secondLocation.Name, Is.EqualTo("Queen St Store"));
        }

        private void AssertSaveBusinessIsNotCalled()
        {
            Assert.That(BusinessRepository.WasSaveBusinessCalled, Is.False);
        }

        private void AssertSaveBusinessIsCalled()
        {
            Assert.That(BusinessRepository.WasSaveBusinessCalled, Is.True);
        }
    }
}
