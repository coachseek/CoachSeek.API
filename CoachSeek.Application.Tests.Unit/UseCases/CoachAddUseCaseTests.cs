using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using NUnit.Framework;
using System;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class CoachAddUseCaseTests : CoachUseCaseTests
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
                BusinessId = new Guid(BUSINESS_ID),
                FirstName = "  Bill",
                LastName = "Gates  ",
                Email = " bgates@Gmail.Com ",
                Phone = "021345678  ",
                WorkingHours = SetupStandardWorkingHoursCommand()
            };
        }

        private CoachAddCommand GivenAUniqueCoach()
        {
            return new CoachAddCommand
            {
                BusinessId = new Guid(BUSINESS_ID),
                FirstName = "Steve  ",
                LastName = "  Jobs",
                Email = "  Steve.Jobs@APPLE.com",
                Phone = "021888888  ",
                WorkingHours = SetupStandardWorkingHoursCommand()
            };
        }

        private Response<CoachData> WhenAddCoach(CoachAddCommand command)
        {
            var useCase = new CoachAddUseCase(BusinessRepository);

            return useCase.AddCoach(command);
        }

        private void ThenCoachAddFailsWithMissingCoachError(Response<CoachData> response)
        {
            AssertMissingCoachError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenCoachAddFailsWithInvalidBusinessError(Response<CoachData> response)
        {
            AssertInvalidBusinessError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenCoachAddFailsWithDuplicateCoachError(Response<CoachData> response)
        {
            AssertDuplicateCoachError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void ThenCoachAddSucceeds(Response<CoachData> response)
        {
            AssertSaveBusinessIsCalled();
            AssertResponseReturnsNewCoach(response);
        }

        private void AssertMissingCoachError(Response<CoachData> response)
        {
            AssertSingleError(response, "Missing coach data.");
        }

        private void AssertInvalidBusinessError(Response<CoachData> response)
        {
            AssertSingleError(response, "This business does not exist.");
        }

        private void AssertDuplicateCoachError(Response<CoachData> response)
        {
            AssertSingleError(response, "This coach already exists.");
        }

        private void AssertResponseReturnsNewCoach(Response<CoachData> response)
        {
            var coach = response.Data;
            Assert.That(coach, Is.Not.Null);
            Assert.That(coach.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(coach.FirstName, Is.EqualTo("Steve"));
            Assert.That(coach.LastName, Is.EqualTo("Jobs"));
            Assert.That(coach.Name, Is.EqualTo("Steve Jobs"));
            Assert.That(coach.Email, Is.EqualTo("steve.jobs@apple.com"));
            Assert.That(coach.Phone, Is.EqualTo("021888888"));
            AssertStandardWorkingHours(coach);
        }
    }
}
