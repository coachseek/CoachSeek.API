using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Repositories;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Services.Builders;
using CoachSeek.Services.Email;
using NUnit.Framework;
using System;
using System.Linq;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class BusinessNewRegistrationUseCaseTests
    {
        private const string BUSINESS_ID = "12345678-90AB-4B1D-B8AA-920DD568681E";

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

            var business = new Business(new Guid(BUSINESS_ID),
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
                null, 
                null);
            BusinessRepository.Add(business);

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
        public void GivenNoRegistration_WhenRegisterANewBusiness_ThenBusinessRegistrationFailsWithMissingRegistrationError()
        {
            var registration = GivenNoRegistration();
            var response = WhenRegisterANewBusiness(registration);
            ThenBusinessRegistrationFailsWithMissingRegistrationError(response);
        }

        //[Test]
        //public void GivenEmptyRegistration_WhenRegisterANewBusiness_ThenBusinessRegistrationFailsWithInvalidRegistrationErrors()
        //{
        //    var registration = GivenEmptyRegistration();
        //    var response = WhenRegisterANewBusiness(registration);
        //    ThenBusinessRegistrationFailsWithInvalidRegistrationErrors(response);
        //}

        [Test]
        public void 
            GivenAnExistingEmailAddress_WhenRegisterANewBusiness_ThenBusinessRegistrationFailsWithDupliateEmailError()
        {
            var registration = GivenAnExistingEmailAddress();
            var response = WhenRegisterANewBusiness(registration);
            ThenBusinessRegistrationFailsWithDupliateEmailError(response);
        }

        [Test]
        public void GivenAUniqueEmailAddress_WhenRegisterANewBusiness_ThenBusinessRegistrationSucceeds()
        {
            var registration = GivenAUniqueEmailAddress();
            var response = WhenRegisterANewBusiness(registration);
            ThenBusinessRegistrationSucceeds(response);
        }

        private BusinessRegistrationCommand GivenNoRegistration()
        {
            return null;
        }

        private BusinessRegistrationCommand GivenEmptyRegistration()
        {
            return new BusinessRegistrationCommand();
        }

        private BusinessRegistrationCommand GivenAnExistingEmailAddress()
        {
            return new BusinessRegistrationCommand
            {
                BusinessName = "Olaf's Cafe  ",
                Registrant = new BusinessRegistrantCommand
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
                Registrant = new BusinessRegistrantCommand
                {
                    FirstName = " Ian",
                    LastName = "Bishop ",
                    Email = "  ianbish@GMAIL.com",
                    Password = "password"
                }
            };
        }

        private Response WhenRegisterANewBusiness(BusinessRegistrationCommand registration)
        {
            var useCase = new BusinessNewRegistrationUseCase(BusinessRepository, 
                BusinessDomainBuilder,
                BusinessRegistrationEmailer);

            return useCase.RegisterNewBusiness(registration);
        }

        private void ThenBusinessRegistrationFailsWithMissingRegistrationError(Response response)
        {
            AssertMissingRegistrationError(response);
            AssertBusinessRegistrationFails();
        }

        //private void ThenBusinessRegistrationFailsWithInvalidRegistrationErrors(BusinessRegistrationResponse response)
        //{
        //    AssertInvalidRegistrationError(response);
        //    AssertBusinessRegistrationFails();
        //}

        private void ThenBusinessRegistrationFailsWithDupliateEmailError(Response response)
        {
            AssertDuplicateBusinessAdminEmailError(response);
            AssertBusinessRegistrationFails();
        }

        private void ThenBusinessRegistrationSucceeds(Response response)
        {
            AssertBusinessIsRegistered();
            AssertRegistrationEmailIsSent();
            AssertRegistrationDataIsPassedToEmailer();
        }

        private void AssertMissingRegistrationError(Response response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1010));
            Assert.That(error.Message, Is.EqualTo("Missing business registration data."));
            Assert.That(error.Field, Is.Null);
        }

        //private void AssertInvalidRegistrationError(BusinessRegistrationResponse response)
        //{
        //    Assert.That(response.Business, Is.Null);
        //    Assert.That(response.Errors, Is.Not.Null);
        //    Assert.That(response.Errors.Count, Is.EqualTo(2));
        //    var firstError = response.Errors.First();
        //    //Assert.That(firstError.Code, Is.EqualTo(1010));
        //    //Assert.That(firstError.Field, Is.Null);
        //    var secondError = response.Errors[1];
        //}

        private void AssertDuplicateBusinessAdminEmailError(Response response)
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
            var admin = business.Admin;
            Assert.That(admin.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(admin.FirstName, Is.EqualTo("Ian"));
            Assert.That(admin.LastName, Is.EqualTo("Bishop"));
            Assert.That(admin.Email, Is.EqualTo("ianbish@gmail.com"));
            Assert.That(admin.Username, Is.EqualTo("ianbish@gmail.com"));
            Assert.That(admin.PasswordHash, Is.EqualTo("password"));
        }
    }
}
