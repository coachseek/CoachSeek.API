using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Domain;
using CoachSeek.Domain.Data;
using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.UseCases.Requests;
using CoachSeek.WebUI.Models.UseCases.Responses;
using CoachSeek.WebUI.Persistence;
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
                    Id = new Guid(VALID_COACH_ID),
                    FirstName = "Bob", 
                    LastName = "Smith", 
                    Email = "bob.smith@example.com",
                    Phone = "021987654"
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
