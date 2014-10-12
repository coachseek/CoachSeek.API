using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.DataAccess.Repositories;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Data;
using CoachSeek.Domain.Entities;
using CoachSeek.WebUI.Models.UseCases.Responses;
using CoachSeek.WebUI.UseCases;
using NUnit.Framework;

namespace CoachSeek.WebUI.Tests.Unit.UseCases
{
    [TestFixture]
    public class CoachUpdateUseCaseTests
    {
        private const string INVALID_BUSINESS_ID = "A031E489-C1D4-4889-A3E0-196930BA35DD";
        private const string VALID_BUSINESS_ID = "87738006-7E54-4FA6-9CF5-7FEAB5940668";
        private const string INVALID_COACH_ID = "854C58B1-71F4-46F1-A617-5AAA04509AA4";
        private const string VALID_COACH_ID = "0EC1B243-DA4A-4B05-B973-EFEE33A1C8C4";

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
            var business = new Business(SetupLocations(), SetupCoaches())
            {
                Id = new Guid(VALID_BUSINESS_ID),
                Name = "Olaf's Bookshoppe",
                Domain = "olafsbookshoppe",
                Admin = new BusinessAdmin("Olaf", "Thielke", "olaft@ihug.co.nz", "Password1")
            };

            return business;
        }


        private IEnumerable<Location> SetupLocations()
        {
            return new List<Location>();
        }

        private IEnumerable<CoachData> SetupCoaches()
        {
            return new List<CoachData>
            {
                new CoachData
                {
                    Id = new Guid("E3CE8AD9-C755-4F06-A930-3B7E30F8B967"),
                    FirstName = "Bob", 
                    LastName = "Smith", 
                    Email = "bob.smith@example.com",
                    Phone = "021987654"
                }, 
                new CoachData
                {
                    Id = new Guid(VALID_COACH_ID),
                    FirstName = "Bill", 
                    LastName = "Gates", 
                    Email = "bill@microsoft.com",
                    Phone = "095286912"
                }, 
            };
        }


        [Test]
        public void GivenNoCoachUpdateRequest_WhenUpdateCoach_ThenCoachUpdateFailsWithMissingCoachError()
        {
            var request = GivenNoCoachUpdateRequest();
            var response = WhenUpdateCoach(request);
            ThenCoachUpdateFailsWithMissingCoachError(response);
        }

        [Test]
        public void GivenNonExistentBusiness_WhenUpdateCoach_ThenCoachUpdateFailsWithInvalidBusinessError()
        {
            var request = GivenNonExistentBusiness();
            var response = WhenUpdateCoach(request);
            ThenCoachUpdateFailsWithInvalidBusinessError(response);
        }

        [Test]
        public void GivenNonExistentCoach_WhenUpdateCoach_ThenCoachUpdateFailsWithInvalidCoachError()
        {
            var request = GivenNonExistentCoach();
            var response = WhenUpdateCoach(request);
            ThenCoachUpdateFailsWithInvalidCoachError(response);
        }

        [Test]
        public void GivenExistingCoachName_WhenUpdateCoach_ThenCoachUpdateFailsWithDuplicateCoachError()
        {
            var request = GivenExistingCoachName();
            var response = WhenUpdateCoach(request);
            ThenCoachUpdateFailsWithDuplicateCoachError(response);
        }

        [Test]
        public void GivenAUniqueCoachName_WhenUpdateCoach_ThenCoachUpdateSucceeds()
        {
            var request = GivenAUniqueCoachName();
            var response = WhenUpdateCoach(request);
            ThenCoachUpdateSucceeds(response);
        }

        private CoachUpdateRequest GivenNoCoachUpdateRequest()
        {
            return null;
        }

        private CoachUpdateRequest GivenNonExistentBusiness()
        {
            return new CoachUpdateRequest
            {
                BusinessId = new Guid(INVALID_BUSINESS_ID)
            };
        }

        private CoachUpdateRequest GivenNonExistentCoach()
        {
            return new CoachUpdateRequest
            {
                BusinessId = new Guid(VALID_BUSINESS_ID),
                CoachId = new Guid(INVALID_COACH_ID),
                FirstName = "Warren",
                LastName = "Buffett",
                Email = "warren@buffet.com",
                Phone = "555 1234"
            };
        }

        private CoachUpdateRequest GivenExistingCoachName()
        {
            return new CoachUpdateRequest
            {
                BusinessId = new Guid(VALID_BUSINESS_ID),
                CoachId = new Guid(VALID_COACH_ID),
                FirstName = "Bob",
                LastName = "Smith",
                Email = "bob.smith@example.com",
                Phone = "021987654"
            };
        }

        private CoachUpdateRequest GivenAUniqueCoachName()
        {
            return new CoachUpdateRequest
            {
                BusinessId = new Guid(VALID_BUSINESS_ID),
                CoachId = new Guid(VALID_COACH_ID),
                FirstName = "Napoleon",
                LastName = "Bonaparte",
                Email = "napoleon@waterloo.com",
                Phone = "18061815"
            };
        }

        private CoachUpdateResponse WhenUpdateCoach(CoachUpdateRequest request)
        {
            var useCase = new CoachUpdateUseCase(BusinessRepository);

            return useCase.UpdateCoach(request);
        }

        private void ThenCoachUpdateFailsWithMissingCoachError(CoachUpdateResponse response)
        {
            AssertMissingCoachError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenCoachUpdateFailsWithInvalidBusinessError(CoachUpdateResponse response)
        {
            AssertInvalidBusinessError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenCoachUpdateFailsWithInvalidCoachError(CoachUpdateResponse response)
        {
            AssertInvalidCoachError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenCoachUpdateFailsWithDuplicateCoachError(CoachUpdateResponse response)
        {
            AssertDuplicateCoachError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenCoachUpdateSucceeds(CoachUpdateResponse response)
        {
            AssertSaveBusinessIsCalled();
            AssertCoachIsUpdated(response);
        }

        private void AssertMissingCoachError(CoachUpdateResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1230));
            Assert.That(error.Message, Is.EqualTo("Missing coach data."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertInvalidBusinessError(CoachUpdateResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1030));
            Assert.That(error.Message, Is.EqualTo("This business does not exist."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertInvalidCoachError(CoachUpdateResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1240));
            Assert.That(error.Message, Is.EqualTo("This coach does not exist."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertDuplicateCoachError(CoachUpdateResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1220));
            Assert.That(error.Message, Is.EqualTo("This coach already exists."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertCoachIsUpdated(CoachUpdateResponse response)
        {
            Assert.That(response.Business, Is.Not.Null);
            Assert.That(response.Business.Coaches.Count, Is.EqualTo(2));

            var firstCoach = response.Business.Coaches[0];
            Assert.That(firstCoach.Id, Is.EqualTo(new Guid("E3CE8AD9-C755-4F06-A930-3B7E30F8B967")));
            Assert.That(firstCoach.FirstName, Is.EqualTo("Bob"));
            Assert.That(firstCoach.LastName, Is.EqualTo("Smith"));
            Assert.That(firstCoach.Name, Is.EqualTo("Bob Smith"));
            Assert.That(firstCoach.Email, Is.EqualTo("bob.smith@example.com"));
            Assert.That(firstCoach.Phone, Is.EqualTo("021987654"));

            var secondCoach = response.Business.Coaches[1];
            Assert.That(secondCoach.Id, Is.EqualTo(new Guid(VALID_COACH_ID)));
            Assert.That(secondCoach.FirstName, Is.EqualTo("Napoleon"));
            Assert.That(secondCoach.LastName, Is.EqualTo("Bonaparte"));
            Assert.That(secondCoach.Name, Is.EqualTo("Napoleon Bonaparte"));
            Assert.That(secondCoach.Email, Is.EqualTo("napoleon@waterloo.com"));
            Assert.That(secondCoach.Phone, Is.EqualTo("18061815"));
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
