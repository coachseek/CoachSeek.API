using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.UseCases;
using CoachSeek.Common;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
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
            SetupUserRepository();
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
            ThenCoachAddSucceeds();
        }


        private CoachAddCommand GivenExistingCoach()
        {
            return new CoachAddCommand
            {
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


        private IResponse WhenAddCoach(CoachAddCommand command)
        {
            var useCase = new CoachAddUseCase();
            var business = new BusinessDetails(new Guid(BUSINESS_ID), "", "", DateTime.UtcNow.AddDays(1));
            var currency = new CurrencyDetails("NZD", "$");
            var businessContext = new BusinessContext(business, currency, BusinessRepository, null, UserRepository);
            var emailContext = new EmailContext(true, false, "", null);
            var context = new ApplicationContext(null, businessContext, emailContext, null, true);
            useCase.Initialise(context);
            return useCase.AddCoach(command);
        }


        private void ThenCoachAddFailsWithDuplicateCoachError(IResponse response)
        {
            AssertDuplicateCoachError(response);

            Assert.That(BusinessRepository.WasAddCoachCalled, Is.False);
        }

        private void ThenCoachAddFailsWithInvalidWorkingHoursError(IResponse response)
        {
            AssertInvalidWorkingHoursError(response);

            Assert.That(BusinessRepository.WasAddCoachCalled, Is.False);
        }

        private void ThenCoachAddSucceeds()
        {
            Assert.That(BusinessRepository.WasAddCoachCalled, Is.True);

            Assert.That(BusinessRepository.BusinessIdPassedIn, Is.EqualTo(new Guid(BUSINESS_ID)));

            var coach = (Coach)BusinessRepository.DataPassedIn;
            Assert.That(coach.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(coach.FirstName, Is.EqualTo("Steve"));
            Assert.That(coach.LastName, Is.EqualTo("Jobs"));
            Assert.That(coach.Name, Is.EqualTo("Steve Jobs"));
            Assert.That(coach.Email, Is.EqualTo("steve.jobs@apple.com"));
            Assert.That(coach.Phone, Is.EqualTo("021888888"));
            AssertStandardWorkingHours(coach);
        }

        private void AssertDuplicateCoachError(IResponse response)
        {
            AssertSingleError(response, ErrorCodes.CoachDuplicate, "Coach 'Bill Gates' already exists.", "Bill Gates");
        }

        private void AssertInvalidWorkingHoursError(IResponse response)
        {
            AssertMultipleErrors(response, new[,] { { ErrorCodes.DailyWorkingHoursInvalid, "Monday working hours are not valid.", "Monday", null },
                                                    { ErrorCodes.DailyWorkingHoursInvalid, "Tuesday working hours are not valid.", "Tuesday", null },
                                                    { ErrorCodes.DailyWorkingHoursInvalid, "Wednesday working hours are not valid.", "Wednesday", null },
                                                    { ErrorCodes.DailyWorkingHoursInvalid, "Thursday working hours are not valid.", "Thursday", null },
                                                    { ErrorCodes.DailyWorkingHoursInvalid, "Friday working hours are not valid.", "Friday", null },
                                                    { ErrorCodes.DailyWorkingHoursInvalid, "Saturday working hours are not valid.", "Saturday", null },
                                                    { ErrorCodes.DailyWorkingHoursInvalid, "Sunday working hours are not valid.", "Sunday", null } });
        }
    }
}
