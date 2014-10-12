﻿using System;
using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.UseCases;
using CoachSeek.DataAccess.Repositories;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Services.Builders;
using CoachSeek.Services.Email;
using NUnit.Framework;
using System.Linq;

namespace CoachSeek.Application.Tests.Unit
{
    [TestFixture]
    public class BusinessNewRegistrationUseCaseTests
    {
        private InMemoryBusinessRepository BusinessRepository { get; set; }
        private BusinessDomainBuilder BusinessDomainBuilder { get; set; }
        private StubBusinessRegistrationEmailer BusinessRegistrationEmailer { get; set; }
        
        [SetUp]
        public void Setup()
        {
            SetupBusinessRepository();
            SetupBusinessDomainBuilder();
            SetupBusinessRegistrationEmailer();
        }

        private void SetupBusinessRepository()
        {
            BusinessRepository = new InMemoryBusinessRepository();
            BusinessRepository.Clear();

            BusinessRepository.Add(new Business
            {
                Name = "Olaf's Bookshoppe",
                Domain = "olafsbookshoppe",
                Admin = new BusinessAdmin("Olaf", "Thielke", "olaft@ihug.co.nz", "Password1")
            });
            BusinessRepository.WasSaveNewBusinessCalled = false;
            BusinessRepository.WasSaveBusinessCalled = false;
        }

        private void SetupBusinessDomainBuilder()
        {
            BusinessDomainBuilder = new BusinessDomainBuilder(new HardCodedReservedDomainRepository(), BusinessRepository);
        }

        private void SetupBusinessRegistrationEmailer()
        {
            BusinessRegistrationEmailer = new StubBusinessRegistrationEmailer();
        }


        [Test]
        public void GivenNoRegistration_WhenRegisterANewBusiness_ThenTheBusinessRegistrationFailsWithMissingRegistrationError()
        {
            var registration = GivenNoRegistration();
            var response = WhenRegisterANewBusiness(registration);
            ThenTheBusinessRegistrationFailsWithMissingRegistrationError(response);
        }

        [Test]
        public void 
            GivenAnExistingEmailAddress_WhenRegisterANewBusiness_ThenTheBusinessRegistrationFailsWithDupliateEmailError()
        {
            var registration = GivenAnExistingEmailAddress();
            var response = WhenRegisterANewBusiness(registration);
            ThenTheBusinessRegistrationFailsWithDupliateEmailError(response);
        }

        [Test]
        public void GivenAUniqueEmailAddress_WhenRegisterANewBusiness_ThenTheBusinessRegistrationSucceeds()
        {
            var registration = GivenAUniqueEmailAddress();
            var response = WhenRegisterANewBusiness(registration);
            ThenTheBusinessRegistrationSucceeds(response);
        }

        private BusinessRegistrationCommand GivenNoRegistration()
        {
            return null;
        }

        private BusinessRegistrationCommand GivenAnExistingEmailAddress()
        {
            return new BusinessRegistrationCommand
            {
                BusinessName = "Olaf's Cafe  ",
                Registrant = new BusinessRegistrant
                {
                    FirstName = " Olaf ",
                    LastName = " Thielke  ",
                    Email = "OLAFT@ihug.co.nz  ",
                    Password = "password"
                }
            };
        }

        private BusinessRegistrationCommand GivenAUniqueEmailAddress()
        {
            return new BusinessRegistrationCommand
            {
                BusinessName = "  Ian's Cafe",
                Registrant = new BusinessRegistrant
                {
                    FirstName = " Ian",
                    LastName = "Bishop ",
                    Email = "  ianbish@GMAIL.com",
                    Password = "password"
                }
            };
        }

        private BusinessRegistrationResponse WhenRegisterANewBusiness(BusinessRegistrationCommand registration)
        {
            var useCase = new BusinessNewRegistrationUseCase(BusinessRepository, 
                BusinessDomainBuilder,
                BusinessRegistrationEmailer);

            return useCase.RegisterNewBusiness(registration);
        }

        private void ThenTheBusinessRegistrationFailsWithMissingRegistrationError(BusinessRegistrationResponse response)
        {
            AssertMissingRegistrationError(response);
            AssertBusinessRegistrationFails();
        }

        private void ThenTheBusinessRegistrationFailsWithDupliateEmailError(BusinessRegistrationResponse response)
        {
            AssertDuplicateBusinessAdminEmailError(response);
            AssertBusinessRegistrationFails();
        }

        private void AssertMissingRegistrationError(BusinessRegistrationResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1010));
            Assert.That(error.Message, Is.EqualTo("Missing business registration data."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertDuplicateBusinessAdminEmailError(BusinessRegistrationResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1020));
            Assert.That(error.Message, Is.EqualTo("This email address is already in use."));
            Assert.That(error.Field, Is.EqualTo("Email"));
        }

        private void AssertBusinessRegistrationFails()
        {
            AssertBusinessIsNotRegistered();
            AssertRegistrationEmailIsNotSent();
        }


        private void ThenTheBusinessRegistrationSucceeds(BusinessRegistrationResponse response)
        {
            AssertBusinessIsRegistered();
            AssertRegistrationEmailIsSent();
            AssertRegistrationDataIsPassedToEmailer();
        }

        private void AssertBusinessIsNotRegistered()
        {
            Assert.That(BusinessRepository.WasSaveNewBusinessCalled, Is.False);
        }

        private void AssertBusinessIsRegistered()
        {
            Assert.That(BusinessRepository.WasSaveNewBusinessCalled, Is.True);
        }

        private void AssertRegistrationEmailIsNotSent()
        {
            Assert.That(BusinessRegistrationEmailer.WasSendEmailCalled, Is.False);
        }

        private void AssertRegistrationEmailIsSent()
        {
            Assert.That(BusinessRegistrationEmailer.WasSendEmailCalled, Is.True);
        }

        private void AssertRegistrationDataIsPassedToEmailer()
        {
            var business = BusinessRegistrationEmailer.PassedInBusinessData;
            Assert.That(business.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(business.Name, Is.EqualTo("Ian's Cafe"));
            Assert.That(business.Domain, Is.EqualTo("ianscafe"));
            var admin = business.BusinessAdmin;
            Assert.That(admin.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(admin.FirstName, Is.EqualTo("Ian"));
            Assert.That(admin.LastName, Is.EqualTo("Bishop"));
            Assert.That(admin.Email, Is.EqualTo("ianbish@gmail.com"));
            Assert.That(admin.Username, Is.EqualTo("ianbish@gmail.com"));
            Assert.That(admin.PasswordHash, Is.EqualTo("password"));
        }
    }
}
