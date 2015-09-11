using System;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Tests.Unit.Fakes;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class NewBusinessTests
    {
        private const string BUSINESS_NAME = "Ian's Tennis Coaching";
        private const string DOMAIN_NAME = "dummydomain";
        private const string SPORT_NAME = "Tennis";
        private const string TESTING_EMAIL = "olaf@coachseek.com";
        private const string LIVE_EMAIL = "olaf@microsoft.com";
        private const string INVALID_CURRENCY = "XXX";
        private const string VALID_CURRENCY = "USD";

        private MockBusinessDomainBuilder BusinessDomainBuilder { get; set; }
        private HardCodedSupportedCurrencyRepository SupportedCurrencyRepository { get; set; }


        [SetUp]
        public void Setup()
        {
            SetupBusinessDomainBuilder();
            SetupSupportedCurrencyRepository();
        }

        private void SetupBusinessDomainBuilder()
        {
            BusinessDomainBuilder = new MockBusinessDomainBuilder { Domain = DOMAIN_NAME };
        }

        private void SetupSupportedCurrencyRepository()
        {
            SupportedCurrencyRepository = new HardCodedSupportedCurrencyRepository();
        }


        [Test]
        public void GivenWhitespaceSurroundingBusinessName_WhenConstructNewBusiness_ThenCreateNewBusinessWithoutWhitespaceInBusinessName()
        {
            var command = GivenWhitespaceSurroundingBusinessName();
            var response = WhenConstructNewBusiness(command);
            ThenCreateNewBusinessWithoutWhitespaceInBusinessName(response);
        }

        [Test]
        public void WhenConstructNewBusiness_ThenCallBusinessDomainBuilder()
        {
            var response = WhenConstructNewBusiness();
            ThenCallBusinessDomainBuilder(response);
        }

        [Test]
        public void GivenNoSportInRegistration_WhenConstructNewBusiness_ThenCreateNewBusinessWithoutSport()
        {
            var command = GivenNoSportInRegistration();
            var response = WhenConstructNewBusiness(command);
            ThenCreateNewBusinessWithoutSport(response);
        }

        [Test]
        public void GivenSportInRegistration_WhenConstructNewBusiness_ThenCreateNewBusinessWithSport()
        {
            var command = GivenSportInRegistration();
            var response = WhenConstructNewBusiness(command);
            ThenCreateNewBusinessWithSport(response);
        }

        [Test]
        public void GivenCoachseekEmailInRegistration_WhenConstructNewBusiness_ThenCreateNewTestingBusiness()
        {
            var command = GivenCoachseekEmailInRegistration();
            var response = WhenConstructNewBusiness(command);
            ThenCreateNewTestingBusiness(response);
        }

        [Test]
        public void GivenNonCoachseekEmailInRegistration_WhenConstructNewBusiness_ThenCreateNewLiveBusiness()
        {
            var command = GivenNonCoachseekEmailInRegistration();
            var response = WhenConstructNewBusiness(command);
            ThenCreateNewLiveBusiness(response);
        }

        [Test]
        public void WhenConstructNewBusiness_ThenSetCreatedOnToNow()
        {
            var response = WhenConstructNewBusiness();
            ThenSetCreatedOnToNow(response);
        }

        [Test]
        public void GivenInvalidCurrencyInRegistration_WhenConstructNewBusiness_ThenThrowsInvalidCurrencyError()
        {
            var command = GivenInvalidCurrencyInRegistration();
            var response = WhenConstructNewBusiness(command);
            ThenThrowsInvalidCurrencyError(response);
        }

        [Test]
        public void GivenValidCurrencyInRegistration_WhenConstructNewBusiness_ThenCreateNewBusinessWithCurrency()
        {
            var command = GivenValidCurrencyInRegistration();
            var response = WhenConstructNewBusiness(command);
            ThenCreateNewBusinessWithCurrency(response);
        }

        [Test]
        public void WhenConstructNewBusiness_ThenSetPaymentDefaults()
        {
            var response = WhenConstructNewBusiness();
            ThenSetPaymentDefaults(response);
        }


        private BusinessRegistrationCommand GivenWhitespaceSurroundingBusinessName()
        {
            var command = GivenValidBusinessRegistrationCommand();
            command.Business.Name = string.Format(" {0}  ", BUSINESS_NAME);
            return command;
        }

        private BusinessRegistrationCommand GivenNoSportInRegistration()
        {
            var command = GivenValidBusinessRegistrationCommand();
            command.Business.Sport = null;
            return command;
        }

        private BusinessRegistrationCommand GivenSportInRegistration()
        {
            var command = GivenValidBusinessRegistrationCommand();
            command.Business.Sport = string.Format("  {0} ", SPORT_NAME);
            return command;
        }

        private BusinessRegistrationCommand GivenCoachseekEmailInRegistration()
        {
            var command = GivenValidBusinessRegistrationCommand();
            command.Admin.Email = TESTING_EMAIL;
            return command;
        }

        private BusinessRegistrationCommand GivenNonCoachseekEmailInRegistration()
        {
            var command = GivenValidBusinessRegistrationCommand();
            command.Admin.Email = LIVE_EMAIL;
            return command;
        }

        private BusinessRegistrationCommand GivenInvalidCurrencyInRegistration()
        {
            var command = GivenValidBusinessRegistrationCommand();
            command.Business.Currency = INVALID_CURRENCY;
            return command;
        }

        private BusinessRegistrationCommand GivenValidCurrencyInRegistration()
        {
            var command = GivenValidBusinessRegistrationCommand();
            command.Business.Currency = VALID_CURRENCY;
            return command;
        }

        private BusinessRegistrationCommand GivenValidBusinessRegistrationCommand()
        {
            return new BusinessRegistrationCommand
            {
                Business = new BusinessAddCommand
                {
                    Name = BUSINESS_NAME,
                    Sport = SPORT_NAME,
                    Currency = "NZD"
                },
                Admin = new UserAddCommand
                {
                    Email = "olaf@coachseek.com"
                }
            };
        }


        private object WhenConstructNewBusiness(BusinessRegistrationCommand command)
        {
            try
            {
                return new NewBusiness(command, BusinessDomainBuilder, SupportedCurrencyRepository);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private object WhenConstructNewBusiness()
        {
            try
            {
                var command = GivenValidBusinessRegistrationCommand();
                return new NewBusiness(command, BusinessDomainBuilder, SupportedCurrencyRepository);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        private void ThenCreateNewBusinessWithoutWhitespaceInBusinessName(object response)
        {
            Assert.That(response, Is.InstanceOf<NewBusiness>());
            var newBusiness = (NewBusiness)response;

            Assert.That(newBusiness.Name, Is.EqualTo(BUSINESS_NAME));
        }

        private void ThenCallBusinessDomainBuilder(object response)
        {
            Assert.That(response, Is.InstanceOf<NewBusiness>());
            var newBusiness = (NewBusiness)response;

            Assert.That(newBusiness.Domain, Is.EqualTo(DOMAIN_NAME));
            Assert.That(BusinessDomainBuilder.WasBuildDomainCalled, Is.True);
        }

        private void ThenCreateNewBusinessWithoutSport(object response)
        {
            Assert.That(response, Is.InstanceOf<NewBusiness>());
            var newBusiness = (NewBusiness)response;

            Assert.That(newBusiness.Sport, Is.Null);
        }

        private void ThenCreateNewBusinessWithSport(object response)
        {
            Assert.That(response, Is.InstanceOf<NewBusiness>());
            var newBusiness = (NewBusiness)response;

            Assert.That(newBusiness.Sport, Is.EqualTo(SPORT_NAME));
        }

        private void ThenCreateNewTestingBusiness(object response)
        {
            Assert.That(response, Is.InstanceOf<NewBusiness>());
            var newBusiness = (NewBusiness)response;

            Assert.That(newBusiness.IsTesting, Is.True);
        }

        private void ThenCreateNewLiveBusiness(object response)
        {
            Assert.That(response, Is.InstanceOf<NewBusiness>());
            var newBusiness = (NewBusiness)response;

            Assert.That(newBusiness.IsTesting, Is.False);
        }

        private void ThenSetCreatedOnToNow(object response)
        {
            Assert.That(response, Is.InstanceOf<NewBusiness>());
            var newBusiness = (NewBusiness)response;

            var now = DateTime.UtcNow;
            Assert.That(newBusiness.CreatedOn, Is.GreaterThan(now.Subtract(new TimeSpan(0, 1, 0))));
            Assert.That(newBusiness.CreatedOn, Is.LessThan(now.Add(new TimeSpan(0, 1, 0))));
        }

        private void ThenThrowsInvalidCurrencyError(object response)
        {
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var exception = (ValidationException)response;

            var errors = exception.Errors;
            Assert.That(errors.Count, Is.EqualTo(1));
            var error = errors[0];
            Assert.That(error.Code, Is.EqualTo("currency-not-supported"));
            Assert.That(error.Message, Is.EqualTo("Currency 'XXX' is not supported."));
            Assert.That(error.Data, Is.EqualTo("XXX"));
        }

        private void ThenCreateNewBusinessWithCurrency(object response)
        {
            Assert.That(response, Is.InstanceOf<NewBusiness>());
            var newBusiness = (NewBusiness)response;

            Assert.That(newBusiness.Currency, Is.EqualTo(VALID_CURRENCY));
        }

        private void ThenSetPaymentDefaults(object response)
        {
            Assert.That(response, Is.InstanceOf<NewBusiness>());
            var newBusiness = (NewBusiness)response;

            Assert.That(newBusiness.IsOnlinePaymentEnabled, Is.False);
            Assert.That(newBusiness.ForceOnlinePayment, Is.False);
            Assert.That(newBusiness.PaymentProvider, Is.Null);
        }
    }
}
