﻿using CoachSeek.Application.Contracts.Models;
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


        private CoachAddCommand GivenNoCoachAddCommand()
        {
            return null;
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


        private Response WhenAddCoach(CoachAddCommand command)
        {
            var useCase = new CoachAddUseCase();
            var business = new BusinessDetails(new Guid(BUSINESS_ID), "", "");
            var currency = new CurrencyDetails("NZD", "$");
            var businessContext = new BusinessContext(business, currency, null, BusinessRepository, null, null);
            var emailContext = new EmailContext(true, false, "", null);
            var context = new ApplicationContext(businessContext, emailContext, true);
            useCase.Initialise(context);
            return useCase.AddCoach(command);
        }


        private void ThenCoachAddFailsWithMissingCoachError(Response response)
        {
            AssertMissingCoachError(response);

            Assert.That(BusinessRepository.WasAddCoachCalled, Is.False);
        }

        private void ThenCoachAddFailsWithDuplicateCoachError(Response response)
        {
            AssertDuplicateCoachError(response);

            Assert.That(BusinessRepository.WasAddCoachCalled, Is.False);
        }

        private void ThenCoachAddFailsWithInvalidWorkingHoursError(Response response)
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

        private void AssertMissingCoachError(Response response)
        {
            AssertSingleError(response, "Missing data.");
        }

        private void AssertDuplicateCoachError(Response response)
        {
            AssertSingleError(response, "This coach already exists.");
        }

        private void AssertInvalidWorkingHoursError(Response response)
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
    }
}
