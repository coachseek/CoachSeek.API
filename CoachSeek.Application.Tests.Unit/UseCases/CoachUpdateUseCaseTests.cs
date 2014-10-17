using CoachSeek.Application.Configuration;
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
    public class CoachUpdateUseCaseTests
    {
        private const string INVALID_BUSINESS_ID = "A031E489-C1D4-4889-A3E0-196930BA35DD";
        private const string VALID_BUSINESS_ID = "87738006-7E54-4FA6-9CF5-7FEAB5940668";
        private const string INVALID_COACH_ID = "854C58B1-71F4-46F1-A617-5AAA04509AA4";
        private const string VALID_COACH_ID = "0EC1B243-DA4A-4B05-B973-EFEE33A1C8C4";

        private InMemoryBusinessRepository BusinessRepository { get; set; }

        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            ApplicationAutoMapperConfigurator.Configure();
        }

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
            return new List<LocationData>();
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
        public void GivenNoCoachUpdateCommand_WhenUpdateCoach_ThenCoachUpdateFailsWithMissingCoachError()
        {
            var request = GivenNoCoachUpdateCommand();
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

        private CoachUpdateCommand GivenNoCoachUpdateCommand()
        {
            return null;
        }

        private CoachUpdateCommand GivenNonExistentBusiness()
        {
            return new CoachUpdateCommand
            {
                BusinessId = new Guid(INVALID_BUSINESS_ID)
            };
        }

        private CoachUpdateCommand GivenNonExistentCoach()
        {
            return new CoachUpdateCommand
            {
                BusinessId = new Guid(VALID_BUSINESS_ID),
                CoachId = new Guid(INVALID_COACH_ID),
                FirstName = "Warren",
                LastName = "Buffett",
                Email = "warren@buffet.com",
                Phone = "555 1234"
            };
        }

        private CoachUpdateCommand GivenExistingCoachName()
        {
            return new CoachUpdateCommand
            {
                BusinessId = new Guid(VALID_BUSINESS_ID),
                CoachId = new Guid(VALID_COACH_ID),
                FirstName = "Bob",
                LastName = "Smith",
                Email = "bob.smith@example.com",
                Phone = "021987654"
            };
        }

        private CoachUpdateCommand GivenAUniqueCoachName()
        {
            return new CoachUpdateCommand
            {
                BusinessId = new Guid(VALID_BUSINESS_ID),
                CoachId = new Guid(VALID_COACH_ID),
                FirstName = "Napoleon",
                LastName = "Bonaparte",
                Email = "napoleon@waterloo.com",
                Phone = "18061815"
            };
        }

        private Response WhenUpdateCoach(CoachUpdateCommand command)
        {
            var useCase = new CoachUpdateUseCase(BusinessRepository);

            return useCase.UpdateCoach(command);
        }

        private void ThenCoachUpdateFailsWithMissingCoachError(Response response)
        {
            AssertMissingCoachError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenCoachUpdateFailsWithInvalidBusinessError(Response response)
        {
            AssertInvalidBusinessError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenCoachUpdateFailsWithInvalidCoachError(Response response)
        {
            AssertInvalidCoachError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenCoachUpdateFailsWithDuplicateCoachError(Response response)
        {
            AssertDuplicateCoachError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenCoachUpdateSucceeds(Response response)
        {
            AssertSaveBusinessIsCalled();
            AssertCoachIsUpdated(response);
        }

        private void AssertMissingCoachError(Response response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1230));
            Assert.That(error.Message, Is.EqualTo("Missing coach data."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertInvalidBusinessError(Response response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1030));
            Assert.That(error.Message, Is.EqualTo("This business does not exist."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertInvalidCoachError(Response response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1240));
            Assert.That(error.Message, Is.EqualTo("This coach does not exist."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertDuplicateCoachError(Response response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1220));
            Assert.That(error.Message, Is.EqualTo("This coach already exists."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertCoachIsUpdated(Response response)
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
