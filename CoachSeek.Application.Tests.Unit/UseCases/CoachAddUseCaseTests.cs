using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Repositories;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using NUnit.Framework;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class CoachAddUseCaseTests
    {
        private const string INVALID_BUSINESS_ID = "A031E489-C1D4-4889-A3E0-196930BA35DD";
        private const string VALID_BUSINESS_ID = "87738006-7E54-4FA6-9CF5-7FEAB5940668";
        private const string LOCATION_ID = "BE94064D-7033-4CF8-9F47-7E118A393C2E";
        private const string COACH_ID = "65FF663E-C858-444B-800D-268D61F17E43";

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
                new LocationData {Id = new Guid(LOCATION_ID), Name = "HQ"}
            };
        }

        private IEnumerable<CoachData> SetupCoaches()
        {
            return new List<CoachData>
            {
                new CoachData
                {
                    Id = new Guid(COACH_ID),
                    FirstName = "Bob", 
                    LastName = "Smith", 
                    Email = "bob.smith@example.com",
                    Phone = "021987654"
                }, 
            };
        }

        [Test]
        public void GivenNoCoachAddCommand_WhenAddCoach_ThenCoachAddFailsWithMissingCoachError()
        {
            var request = GivenNoCoachAddCommand();
            var response = WhenAddCoach(request);
            ThenCoachAddFailsWithMissingCoachError(response);
        }

        [Test]
        public void GivenNonExistentBusiness_WhenAddCoach_ThenCoachAddFailsWithInvalidBusinessError()
        {
            var request = GivenNonExistentBusiness();
            var response = WhenAddCoach(request);
            ThenCoachAddFailsWithInvalidBusinessError(response);
        }

        [Test]
        public void GivenExistingCoach_WhenAddCoach_ThenCoachAddFailsWithDuplicateCoachError()
        {
            var request = GivenExistingCoach();
            var response = WhenAddCoach(request);
            ThenCoachAddFailsWithDuplicateCoachError(response);
        }

        [Test]
        public void GivenAUniqueCoach_WhenAddCoach_ThenCoachAddSucceeds()
        {
            var request = GivenAUniqueCoach();
            var response = WhenAddCoach(request);
            ThenCoachAddSucceeds(response);
        }

        private CoachAddCommand GivenNoCoachAddCommand()
        {
            return null;
        }

        private CoachAddCommand GivenNonExistentBusiness()
        {
            return new CoachAddCommand
            {
                BusinessId = new Guid(INVALID_BUSINESS_ID)
            };
        }

        private CoachAddCommand GivenExistingCoach()
        {
            return new CoachAddCommand
            {
                BusinessId = new Guid(VALID_BUSINESS_ID),
                FirstName = "  BOB",
                LastName = "smith  ",
                Email = " Bob.Smith@Test.Com ",
                Phone = "021345678  "
            };
        }

        private CoachAddCommand GivenAUniqueCoach()
        {
            return new CoachAddCommand
            {
                BusinessId = new Guid(VALID_BUSINESS_ID),
                FirstName = "Steve  ",
                LastName = "  Jobs",
                Email = "  Steve.Jobs@APPLE.com",
                Phone = "021888888  "
            };
        }

        private CoachAddResponse WhenAddCoach(CoachAddCommand command)
        {
            var useCase = new CoachAddUseCase(BusinessRepository);

            return useCase.AddCoach(command);
        }

        private void ThenCoachAddFailsWithMissingCoachError(CoachAddResponse response)
        {
            AssertMissingCoachError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenCoachAddFailsWithInvalidBusinessError(CoachAddResponse response)
        {
            AssertInvalidBusinessError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenCoachAddFailsWithDuplicateCoachError(CoachAddResponse response)
        {
            AssertDuplicateCoachError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenCoachAddSucceeds(CoachAddResponse response)
        {
            AssertSaveBusinessIsCalled();
            AssertSavedBusinessWithTwoCoaches(response);
        }

        private void AssertMissingCoachError(CoachAddResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1210));
            Assert.That(error.Message, Is.EqualTo("Missing coach data."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertInvalidBusinessError(CoachAddResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1030));
            Assert.That(error.Message, Is.EqualTo("This business does not exist."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertDuplicateCoachError(CoachAddResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1220));
            Assert.That(error.Message, Is.EqualTo("This coach already exists."));
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
        
        private void AssertSavedBusinessWithTwoCoaches(CoachAddResponse response)
        {
            Assert.That(response.Business, Is.Not.Null);
            Assert.That(response.Business.Coaches.Count, Is.EqualTo(2));

            var firstCoach = response.Business.Coaches[0];
            Assert.That(firstCoach.Id, Is.EqualTo(new Guid(COACH_ID)));
            Assert.That(firstCoach.FirstName, Is.EqualTo("Bob"));
            Assert.That(firstCoach.LastName, Is.EqualTo("Smith"));
            Assert.That(firstCoach.Name, Is.EqualTo("Bob Smith"));
            Assert.That(firstCoach.Email, Is.EqualTo("bob.smith@example.com"));
            Assert.That(firstCoach.Phone, Is.EqualTo("021987654"));

            var secondCoach = response.Business.Coaches[1];
            Assert.That(secondCoach.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(secondCoach.FirstName, Is.EqualTo("Steve"));
            Assert.That(secondCoach.LastName, Is.EqualTo("Jobs"));
            Assert.That(secondCoach.Name, Is.EqualTo("Steve Jobs"));
            Assert.That(secondCoach.Email, Is.EqualTo("steve.jobs@apple.com"));
            Assert.That(secondCoach.Phone, Is.EqualTo("021888888"));
        }
    }
}
