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
        public void GivenInvalidWorkingHours_WhenAddCoach_ThenCoachAddFailsWithInvalidWorkingHoursError()
        {
            var request = GivenInvalidWorkingHours();
            var response = WhenAddCoach(request);
            ThenCoachAddFailsWithInvalidWorkingHoursError(response);
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

        private CoachAddCommand GivenInvalidWorkingHours()
        {
            return new CoachAddCommand
            {
                BusinessId = new Guid(BUSINESS_ID),
                FirstName = "Bob",
                LastName = "Marley",
                Email = "  Bob.Marley@wailers.com",
                Phone = "0215555555",
                WorkingHours = SetupInvalidWorkingHoursCommand()
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

        private WeeklyWorkingHoursCommand SetupInvalidWorkingHoursCommand()
        {
            return new WeeklyWorkingHoursCommand
            {
                Monday = new DailyWorkingHoursCommand(true, "91:00", ":00"),
                Tuesday = new DailyWorkingHoursCommand(true, "abc", "17:00"),
                Wednesday = new DailyWorkingHoursCommand(true, "9:00", "fred"),
                Thursday = new DailyWorkingHoursCommand(true, "hello", "world"),
                Friday = new DailyWorkingHoursCommand(true, "9:00", "12:34:56"),
                Saturday = new DailyWorkingHoursCommand(true, "-1:00", "5:00"),
                Sunday = new DailyWorkingHoursCommand(true, "7:15", "3:33")
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

        private void ThenCoachAddFailsWithInvalidWorkingHoursError(Response<CoachData> response)
        {
            AssertInvalidWorkingHoursError(response);
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
            AssertSingleError(response, "This business does not exist.", "coach.businessId");
        }

        private void AssertDuplicateCoachError(Response<CoachData> response)
        {
            AssertSingleError(response, "This coach already exists.");
        }

        private void AssertInvalidWorkingHoursError(Response<CoachData> response)
        {
            Assert.That(response.Data, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(7));

            AssertError(response.Errors[0], "The monday working hours are not valid.", "coach.workingHours.monday");
            AssertError(response.Errors[1], "The tuesday working hours are not valid.", "coach.workingHours.tuesday");
            AssertError(response.Errors[2], "The wednesday working hours are not valid.", "coach.workingHours.wednesday");
            AssertError(response.Errors[3], "The thursday working hours are not valid.", "coach.workingHours.thursday");
            AssertError(response.Errors[4], "The friday working hours are not valid.", "coach.workingHours.friday");
            AssertError(response.Errors[5], "The saturday working hours are not valid.", "coach.workingHours.saturday");
            AssertError(response.Errors[6], "The sunday working hours are not valid.", "coach.workingHours.sunday");
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
