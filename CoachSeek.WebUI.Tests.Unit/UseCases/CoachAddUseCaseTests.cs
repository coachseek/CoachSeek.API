using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.UseCases.Requests;
using CoachSeek.WebUI.Models.UseCases.Responses;
using CoachSeek.WebUI.Persistence;
using CoachSeek.WebUI.UseCases;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.WebUI.Tests.Unit.UseCases
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
            return new List<Location>
            {
                new Location {Id = new Guid(LOCATION_ID), Name = "HQ"}
            };
        }

        private IEnumerable<Coach> SetupCoaches()
        {
            return new List<Coach>
            {
                new Coach(new Guid(COACH_ID), "Bob", "Smith", "bob.smith@example.com", "021987654")
            };
        }

        [Test]
        public void GivenNoCoachAddRequest_WhenAddCoach_ThenCoachAddFailsWithMissingCoachError()
        {
            var request = GivenNoCoachAddRequest();
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

        private CoachAddRequest GivenNoCoachAddRequest()
        {
            return null;
        }

        private CoachAddRequest GivenNonExistentBusiness()
        {
            return new CoachAddRequest
            {
                BusinessId = new Guid(INVALID_BUSINESS_ID)
            };
        }

        private CoachAddRequest GivenExistingCoach()
        {
            return new CoachAddRequest
            {
                BusinessId = new Guid(VALID_BUSINESS_ID),
                FirstName = "  BOB",
                LastName = "smith  ",
                Email = " Bob.Smith@Test.Com ",
                Phone = "021345678  "
            };
        }

        private CoachAddRequest GivenAUniqueCoach()
        {
            return new CoachAddRequest
            {
                BusinessId = new Guid(VALID_BUSINESS_ID),
                FirstName = "Steve  ",
                LastName = "  Jobs",
                Email = "  Steve.Jobs@APPLE.com",
                Phone = "021888888  "
            };
        }

        private CoachAddResponse WhenAddCoach(CoachAddRequest request)
        {
            var useCase = new CoachAddUseCase(BusinessRepository);

            return useCase.AddCoach(request);
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
