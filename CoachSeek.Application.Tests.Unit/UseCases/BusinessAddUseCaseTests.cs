using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Repositories;
using CoachSeek.Domain.Commands;
using CoachSeek.Services.Builders;
using CoachSeek.Services.Email;
using NUnit.Framework;
using System;
using System.Linq;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class BusinessAddUseCaseTests : UseCaseTests
    {
        private BusinessDomainBuilder BusinessDomainBuilder { get; set; }
        private StubBusinessRegistrationEmailer BusinessRegistrationEmailer { get; set; }

        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            ConfigureAutoMapper();
        }

        [SetUp]
        public void Setup()
        {
            SetupUserRepository();
            SetupBusinessRepository();
            SetupBusinessDomainBuilder();
            SetupBusinessRegistrationEmailer();
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
        public void GivenNoBusinessAddCommand_WhenAddBusiness_ThenBusinessAddFailsWithMissingBusinessError()
        {
            var command = GivenNoBusinessAddCommand();
            var response = WhenAddBusiness(command);
            ThenBusinessAddFailsWithMissingBusinessError(response);
        }

        //[Test]
        //public void GivenBusinessNameTooLong_WhenAddBusiness_ThenBusinessAddFailsWithMissingBusinessError()
        //{
        //    var command = GivenNoBusinessAddCommand();
        //    var response = WhenAddBusiness(command);
        //    ThenBusinessAddFailsWithMissingBusinessError(response);
        //}

        [Test]
        public void GivenAValidBusinessName_WhenAddBusiness_ThenBusinessAddSucceeds()
        {
            var command = GivenAValidBusinessName();
            var response = WhenAddBusiness(command);
            //ThenBusinessAddSucceeds(response);
        }

        private BusinessAddCommand GivenNoBusinessAddCommand()
        {
            return null;
        }

        private BusinessAddCommand GivenAValidBusinessName()
        {
            return new BusinessAddCommand
            {
                Name = "  Ian's Tennis Coaching"
            };
        }

        private Response<BusinessData> WhenAddBusiness(BusinessAddCommand command)
        {
            var useCase = new BusinessAddUseCase(BusinessRepository, BusinessDomainBuilder);

            return useCase.AddBusiness(command);
        }

        private void ThenBusinessAddFailsWithMissingBusinessError(Response<BusinessData> response)
        {
            AssertMissingRegistrationError(response);
            AssertBusinessRegistrationFails();
        }

        //private void ThenBusinessAddSucceeds(Response<BusinessData> response)
        //{
        //    AssertBusinessIsRegistered();
        //    AssertRegistrationEmailIsSent();
        //    AssertRegistrationDataIsPassedToEmailer();
        //}

        private void AssertMissingRegistrationError(Response<BusinessData> response)
        {
            Assert.That(response.Data, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Message, Is.EqualTo("Missing business registration data."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertDuplicateBusinessAdminEmailError(Response<BusinessData> response)
        {
            Assert.That(response.Data, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Message, Is.EqualTo("This email address is already in use."));
            Assert.That(error.Field, Is.EqualTo("registration.registrant.email"));
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

        //private void AssertRegistrationDataIsPassedToEmailer()
        //{
        //    var business = BusinessRegistrationEmailer.PassedInBusinessData;
        //    Assert.That(business.Id, Is.Not.EqualTo(Guid.Empty));
        //    Assert.That(business.Name, Is.EqualTo("Ian's Tennis Coaching"));
        //    Assert.That(business.Domain, Is.EqualTo("ianstenniscoaching"));
        //    var admin = business.Admin;
        //    Assert.That(admin.Id, Is.Not.EqualTo(Guid.Empty));
        //    Assert.That(admin.FirstName, Is.EqualTo("Ian"));
        //    Assert.That(admin.LastName, Is.EqualTo("Bishop"));
        //    Assert.That(admin.Email, Is.EqualTo("ianbish@gmail.com"));
        //    Assert.That(admin.Username, Is.EqualTo("ianbish@gmail.com"));
        //    Assert.That(admin.PasswordHash, Is.EqualTo("password"));
        //}
    }
}
