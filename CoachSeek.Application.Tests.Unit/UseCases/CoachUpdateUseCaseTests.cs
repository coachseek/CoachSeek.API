using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using NUnit.Framework;
using System;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class CoachUpdateUseCaseTests : CoachUseCaseTests
    {
        private const string COACH_WARREN_ID = "854C58B1-71F4-46F1-A617-5AAA04509AA4";

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
                BusinessId = new Guid(BUSINESS_ID),
                CoachId = new Guid(COACH_WARREN_ID),
                FirstName = "Warren",
                LastName = "Buffett",
                Email = "warren@buffet.com",
                Phone = "555 1234"
            };
        }

        private CoachUpdateCommand GivenExistingCoachName()
        {
            // Note: We are intentionally using Albert's Id and Bill's Name.
            return new CoachUpdateCommand
            {
                BusinessId = new Guid(BUSINESS_ID),
                CoachId = new Guid(COACH_ALBERT_ID),
                FirstName = "Bill",
                LastName = "Gates",
                Email = "william@gates.com",
                Phone = "021987654",
                WorkingHours = SetupStandardWorkingHoursCommand()
            };
        }

        private CoachUpdateCommand GivenAUniqueCoachName()
        {
            return new CoachUpdateCommand
            {
                BusinessId = new Guid(BUSINESS_ID),
                CoachId = new Guid(COACH_ALBERT_ID),
                FirstName = "Albert E.",
                LastName = "Neuman",
                Email = "albert@mad.com",
                Phone = "1234567890",
                WorkingHours = SetupWeekendWorkingHoursCommand()
            };
        }

        private Response<CoachData> WhenUpdateCoach(CoachUpdateCommand command)
        {
            var useCase = new CoachUpdateUseCase(BusinessRepository);

            return useCase.UpdateCoach(command);
        }

        private void ThenCoachUpdateFailsWithMissingCoachError(Response<CoachData> response)
        {
            AssertMissingCoachError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenCoachUpdateFailsWithInvalidBusinessError(Response<CoachData> response)
        {
            AssertInvalidBusinessError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenCoachUpdateFailsWithInvalidCoachError(Response<CoachData> response)
        {
            AssertInvalidCoachError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenCoachUpdateFailsWithDuplicateCoachError(Response<CoachData> response)
        {
            AssertDuplicateCoachError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenCoachUpdateSucceeds(Response<CoachData> response)
        {
            AssertSaveBusinessIsCalled();
            AssertResponseReturnsUpdatedCoach(response);
        }


        private void AssertMissingCoachError(Response<CoachData> response)
        {
            AssertSingleError(response, "Missing coach data.");
        }

        private void AssertInvalidBusinessError(Response<CoachData> response)
        {
            AssertSingleError(response, "This business does not exist.");
        }

        private void AssertInvalidCoachError(Response<CoachData> response)
        {
            AssertSingleError(response, "This coach does not exist.");
        }

        private void AssertDuplicateCoachError(Response<CoachData> response)
        {
            AssertSingleError(response, "This coach already exists.");
        }

        private void AssertResponseReturnsUpdatedCoach(Response<CoachData> response)
        {
            var coach = response.Data;
            Assert.That(coach, Is.Not.Null);
            Assert.That(coach.Id, Is.EqualTo(new Guid(COACH_ALBERT_ID)));
            Assert.That(coach.FirstName, Is.EqualTo("Albert E."));
            Assert.That(coach.LastName, Is.EqualTo("Neuman"));
            Assert.That(coach.Name, Is.EqualTo("Albert E. Neuman"));
            Assert.That(coach.Email, Is.EqualTo("albert@mad.com"));
            Assert.That(coach.Phone, Is.EqualTo("1234567890"));
            AssertWeekendWorkingHours(coach);
        }
    }
}
