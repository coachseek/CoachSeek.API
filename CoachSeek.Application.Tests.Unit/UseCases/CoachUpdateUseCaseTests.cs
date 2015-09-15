using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.UseCases;
using CoachSeek.Common;
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
            SetupUserRepository();
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
        public void GivenInvalidWorkingHours_Update_ThenCoachUpdateFailsWithInvalidWorkingHoursError()
        {
            var request = GivenInvalidWorkingHours();
            var response = WhenUpdateCoach(request);
            ThenCoachUpdateFailsWithInvalidWorkingHoursError(response);
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

        private CoachUpdateCommand GivenNonExistentCoach()
        {
            return new CoachUpdateCommand
            {
                Id = new Guid(COACH_WARREN_ID),
                FirstName = "Warren",
                LastName = "Buffett",
                Email = "warren@buffet.com",
                Phone = "555 1234",
                WorkingHours = SetupStandardWorkingHoursCommand()
            };
        }

        private CoachUpdateCommand GivenExistingCoachName()
        {
            // Note: We are intentionally using Albert's Id and Bill's Name.
            return new CoachUpdateCommand
            {
                Id = new Guid(COACH_ALBERT_ID),
                FirstName = "Bill",
                LastName = "Gates",
                Email = "william@gates.com",
                Phone = "021987654",
                WorkingHours = SetupStandardWorkingHoursCommand()
            };
        }

        private CoachUpdateCommand GivenInvalidWorkingHours()
        {
            return new CoachUpdateCommand
            {
                Id = new Guid(COACH_ALBERT_ID),
                FirstName = "Albert E.",
                LastName = "Neuman",
                Email = "albert@mad.com",
                Phone = "1234567890",
                WorkingHours = SetupInvalidWorkingHoursCommand()
            };
        }

        private CoachUpdateCommand GivenAUniqueCoachName()
        {
            return new CoachUpdateCommand
            {
                Id = new Guid(COACH_ALBERT_ID),
                FirstName = "Albert E.",
                LastName = "Neuman",
                Email = "albert@mad.com",
                Phone = "1234567890",
                WorkingHours = SetupWeekendWorkingHoursCommand()
            };
        }

        private WeeklyWorkingHoursCommand SetupInvalidWorkingHoursCommand()
        {
            return new WeeklyWorkingHoursCommand
            {
                Monday = new DailyWorkingHoursCommand(true, "91:00", ":00"),        // NOT VALID 
                Tuesday = new DailyWorkingHoursCommand(true, "9:00", "17:00"),      // VALID
                Wednesday = new DailyWorkingHoursCommand(false, "9:00", "fred"),    // NOT VALID
                Thursday = new DailyWorkingHoursCommand(true, "hello", "world"),    // NOT VALID
                Friday = new DailyWorkingHoursCommand(true, "9:00", "12:45"),       // VALID
                Saturday = new DailyWorkingHoursCommand(false, "0:00", "8:00"),     // VALID
                Sunday = new DailyWorkingHoursCommand(true, "7:15", "3:33")         // NOT VALID
            };
        }


        private IResponse WhenUpdateCoach(CoachUpdateCommand command)
        {
            var useCase = new CoachUpdateUseCase();
            var business = new BusinessDetails(new Guid(BUSINESS_ID), "", "", DateTime.UtcNow.AddDays(1));
            var currency = new CurrencyDetails("NZD", "$");
            var businessContext = new BusinessContext(business, currency, BusinessRepository, null, UserRepository);
            var emailContext = new EmailContext(true, false, "", null);
            var context = new ApplicationContext(null, businessContext, emailContext, null, true);
            useCase.Initialise(context);
            return useCase.UpdateCoach(command);
        }

        private void ThenCoachUpdateFailsWithInvalidCoachError(IResponse response)
        {
            AssertInvalidCoachError(response);

            Assert.That(BusinessRepository.WasUpdateCoachCalled, Is.False);
        }

        private void ThenCoachUpdateFailsWithDuplicateCoachError(IResponse response)
        {
            AssertDuplicateCoachError(response);

            Assert.That(BusinessRepository.WasUpdateCoachCalled, Is.False);
        }

        private void ThenCoachUpdateFailsWithInvalidWorkingHoursError(IResponse response)
        {
            AssertInvalidWorkingHoursError(response);

            Assert.That(BusinessRepository.WasUpdateCoachCalled, Is.False);
        }

        private void ThenCoachUpdateSucceeds(IResponse response)
        {
            Assert.That(BusinessRepository.WasUpdateCoachCalled, Is.True);

            AssertResponseReturnsUpdatedCoach(response);
        }


        private void AssertMissingCoachError(IResponse response)
        {
            AssertSingleError(response, ErrorCodes.DataRequired, "Missing data.");
        }

        private void AssertInvalidCoachError(IResponse response)
        {
            AssertSingleError(response, ErrorCodes.CoachInvalid, "This coach does not exist.", COACH_WARREN_ID.ToLower());
        }

        private void AssertDuplicateCoachError(IResponse response)
        {
            AssertSingleError(response, ErrorCodes.CoachDuplicate, "Coach 'Bill Gates' already exists.", "Bill Gates");
        }

        private void AssertInvalidWorkingHoursError(IResponse response)
        {
            AssertMultipleErrors(response, new[,] { { ErrorCodes.DailyWorkingHoursInvalid, "Monday working hours are not valid.", "Monday", null },
                                                    { ErrorCodes.DailyWorkingHoursInvalid, "Wednesday working hours are not valid.", "Wednesday", null },
                                                    { ErrorCodes.DailyWorkingHoursInvalid, "Thursday working hours are not valid.", "Thursday", null },
                                                    { ErrorCodes.DailyWorkingHoursInvalid, "Sunday working hours are not valid.", "Sunday", null } });
        }

        private void AssertResponseReturnsUpdatedCoach(IResponse response)
        {
            var coach = (CoachData)response.Data;
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
