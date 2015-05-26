using System;
using System.Linq;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.UseCases;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Services;
using NUnit.Framework;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class BusinessAddUseCaseTests : UseCaseTests
    {
        private BusinessDomainBuilder BusinessDomainBuilder { get; set; }

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
        }

        private void SetupBusinessDomainBuilder()
        {
            BusinessDomainBuilder = new BusinessDomainBuilder(new HardCodedReservedDomainRepository()) { BusinessRepository = BusinessRepository };
        }


        [Test]
        public void GivenNoBusinessAddCommand_WhenAddBusiness_ThenBusinessAddFailsWithMissingBusinessError()
        {
            var command = GivenNoBusinessAddCommand();
            var response = WhenAddBusiness(command);
            ThenBusinessAddFailsWithMissingBusinessError(response);
        }

        [Test]
        public void GivenAValidBusinessName_WhenAddBusiness_ThenBusinessAddSucceeds()
        {
            var command = GivenAValidBusinessName();
            var response = WhenAddBusiness(command);
            ThenBusinessAddSucceeds();
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

        private Response WhenAddBusiness(BusinessAddCommand command)
        {
            var useCase = new BusinessAddUseCase(BusinessDomainBuilder, new HardCodedSupportedCurrencyRepository());

            var businessContext = new BusinessContext(null, "", BusinessRepository, null, null);
            var emailContext = new EmailContext(true, false, "", null);
            var context = new ApplicationContext(businessContext, emailContext, true);
            useCase.Initialise(context);
            return useCase.AddBusiness(command);
        }

        private void ThenBusinessAddFailsWithMissingBusinessError(Response response)
        {
            AssertMissingRegistrationError(response);
            AssertBusinessRegistrationFails();
        }

        private void ThenBusinessAddSucceeds()
        {
            AssertBusinessIsRegistered();
        }

        private void AssertMissingRegistrationError(Response response)
        {
            Assert.That(response.Data, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Message, Is.EqualTo("Missing business registration data."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertBusinessRegistrationFails()
        {
            AssertBusinessIsNotRegistered();
        }

        private void AssertBusinessIsNotRegistered()
        {
            Assert.That(BusinessRepository.WasAddBusinessCalled, Is.False);
        }

        private void AssertBusinessIsRegistered()
        {
            Assert.That(BusinessRepository.WasAddBusinessCalled, Is.True);

            var businessData = (Business)BusinessRepository.DataPassedIn;
            Assert.That(businessData.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(businessData.Name, Is.EqualTo("Ian's Tennis Coaching"));
        }
    }
}
