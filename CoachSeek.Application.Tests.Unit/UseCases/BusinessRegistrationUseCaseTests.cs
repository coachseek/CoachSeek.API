using System;
using CoachSeek.Application.Configuration;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Tests.Unit.Fakes;
using CoachSeek.Application.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Authentication.Repositories;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class BusinessRegistrationUseCaseConstructionTests
    {
        [Test]
        public void WhenConstruct_ThenSetDependencies()
        {
            var useCase = WhenConstruct();
            ThenSetDependencies(useCase);
        }

        private BusinessRegistrationUseCase WhenConstruct()
        {
            return new BusinessRegistrationUseCase(new MockUserAddUseCase(), 
                                                   new MockBusinessAddUseCase(), 
                                                   new MockUserAssociateWithBusinessUseCase())
            {
                UserRepository = new InMemoryUserRepository(),
                BusinessRepository = new InMemoryBusinessRepository(),
                SupportedCurrencyRepository = new HardCodedSupportedCurrencyRepository()
            };
        }

        private void ThenSetDependencies(BusinessRegistrationUseCase useCase)
        {
            Assert.That(useCase.UserAddUseCase, Is.Not.Null);
            Assert.That(useCase.BusinessAddUseCase, Is.Not.Null);
            Assert.That(useCase.UserAssociateWithBusinessUseCase, Is.Not.Null);
        }
    }


    [TestFixture]
    public class BusinessRegistrationUseCaseTests
    {
        private const string USER_ID = "87654321-F027-990A-EDF0-ADED12F4880B";
        private const string BUSINESS_ID = "12345678-B10E-4C0E-B46F-1386B98CE567";
        private const string BUSINESS_NAME = "Olaf's Scuba Shoppe";
        private const string BUSINESS_DOMAIN = "olafsscubashoppe";

        private BusinessRegistrationUseCase UseCase { get; set; }
        private MockUserAddUseCase UserAddUseCase { get; set; }
        private MockBusinessAddUseCase BusinessAddUseCase { get; set; }
        private MockUserAssociateWithBusinessUseCase AssociateUseCase { get; set; }

        private UserData UserData { get; set; }
        private BusinessData BusinessData { get; set; }


        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            ApplicationAutoMapperConfigurator.Configure();
        }

        [SetUp]
        public void Setup()
        {
            SetupUserAddUseCase();
            SetupBusinessAddUseCase();
            SetupAssociateUseCase();

            SetupUseCase();
        }

        private void SetupUseCase()
        {
            UseCase = new BusinessRegistrationUseCase(UserAddUseCase, BusinessAddUseCase, AssociateUseCase)
            {
                UserRepository = new InMemoryUserRepository(),
                BusinessRepository = new InMemoryBusinessRepository(),
                SupportedCurrencyRepository = new HardCodedSupportedCurrencyRepository()
            };
        }

        private void SetupUserAddUseCase()
        {
            UserData = new UserData
            {
                Id = new Guid(USER_ID),
                FirstName = "Olaf",
                LastName = "Thielke",
                Email = "olaf@gmail.com",
                Username = "olaf@gmail.com"
            };

            UserAddUseCase = new MockUserAddUseCase
            {
                Response = new Response(UserData)
            };
        }

        private void SetupBusinessAddUseCase()
        {
            BusinessData = new BusinessData
            {
                Id = new Guid(BUSINESS_ID),
                Name = BUSINESS_NAME,
                Domain = BUSINESS_DOMAIN
            };

            BusinessAddUseCase = new MockBusinessAddUseCase
            {
                Response = new Response(BusinessData)
            };
        }

        private void SetupAssociateUseCase()
        {
            UserData = new UserData
            {
                Id = new Guid(USER_ID),
                BusinessId = new Guid(BUSINESS_ID),
                BusinessName = BUSINESS_NAME,
                FirstName = "Olaf",
                LastName = "Thielke",
                Email = "olaf@gmail.com",
                Username = "olaf@gmail.com"
            };

            AssociateUseCase = new MockUserAssociateWithBusinessUseCase
            {
                Response = new Response(UserData)
            };
        }

        private BusinessRegistrationCommand Command
        {
            get
            {
                return new BusinessRegistrationCommand
                {
                    Business = new BusinessAddCommand
                    {
                        Name = BUSINESS_NAME,
                        Currency = "NZD"
                    },
                    Admin = new UserAddCommand
                    {
                        FirstName = "Olaf",
                        LastName = "Thielke",
                        Email = "olaf@gmail.com",
                        Password = "password1"
                    }
                };
            }
        }


        [Test]
        public void GivenUserAddUseCaseFails_WhenRegisterBusiness_ThenFailAndEndWorkflowAfterUserAddUseCase()
        {
            GivenUserAddUseCaseFails();
            var response = WhenRegisterBusiness();
            ThenFailAndEndWorkflowAfterUserAddUseCase(response);
        }

        [Test]
        public void GivenBusinessAddUseCaseFails_WhenRegisterBusiness_ThenFailAndEndWorkflowAfterBusinessAddUseCase()
        {
            GivenBusinessAddUseCaseFails();
            var response = WhenRegisterBusiness();
            ThenFailAndEndWorkflowAfterBusinessAddUseCase(response);
        }

        [Test]
        public void GivenAssociateUseCaseFails_WhenRegisterBusiness_ThenFailAndEndWorkflowAfterAssociateUseCase()
        {
            GivenAssociateUseCaseFails();
            var response = WhenRegisterBusiness();
            ThenFailAndEndWorkflowAfterAssociateUseCase(response);
        }

        [Test]
        public void GivenAllUseCasesSucceed_WhenRegisterBusiness_ThenSucceedsAndCompletesFullWorkflow()
        {
            GivenAllUseCasesSucceed();
            var response = WhenRegisterBusiness();
            ThenSucceedsAndCompletesFullWorkflow(response);
        }


        private void GivenUserAddUseCaseFails()
        {
            UserAddUseCase.Exception = new SingleErrorException("UserAddUseCase Error");
        }

        private void GivenBusinessAddUseCaseFails()
        {
            BusinessAddUseCase.Exception = new SingleErrorException("BusinessAddUseCase Error");
        }

        private void GivenAssociateUseCaseFails()
        {
            AssociateUseCase.Exception = new ValidationException("AssociateUseCase Error");
        }

        private void GivenAllUseCasesSucceed()
        {
            // Already all set up.
        }


        private IResponse WhenRegisterBusiness()
        {
            return UseCase.RegisterBusiness(Command);
        }

        private void ThenFailAndEndWorkflowAfterUserAddUseCase(IResponse response)
        {
            AssertWasAddUserCalled(true);
            AssertPassRelevantInfoIntoAddUser();

            AssertWasAddBusinessCalled(false);
            AssertWasAssociateUserWithBusinessCalled(false);

            AssertErrorResponse(response);
        }

        private void ThenFailAndEndWorkflowAfterBusinessAddUseCase(IResponse response)
        {
            AssertWasAddUserCalled(true);
            AssertPassRelevantInfoIntoAddUser();

            AssertWasAddBusinessCalled(true);
            AssertPassRelevantInfoIntoAddBusiness();

            AssertWasAssociateUserWithBusinessCalled(false);

            AssertErrorResponse(response);
        }

        private void ThenFailAndEndWorkflowAfterAssociateUseCase(IResponse response)
        {
            AssertWasAddUserCalled(true);
            AssertPassRelevantInfoIntoAddUser();

            AssertWasAddBusinessCalled(true);
            AssertPassRelevantInfoIntoAddBusiness();

            AssertWasAssociateUserWithBusinessCalled(true);
            AssertPassRelevantInfoIntoAssociateUserWithBusiness();

            AssertErrorResponse(response);
        }

        private void ThenSucceedsAndCompletesFullWorkflow(IResponse response)
        {
            AssertWasAddUserCalled(true);
            AssertPassRelevantInfoIntoAddUser();

            AssertWasAddBusinessCalled(true);
            AssertPassRelevantInfoIntoAddBusiness();

            AssertWasAssociateUserWithBusinessCalled(true);
            AssertPassRelevantInfoIntoAssociateUserWithBusiness();

            AssertSuccessResponse(response);
        }


        private void AssertWasAddUserCalled(bool wasCalled)
        {
            Assert.That(UserAddUseCase.WasAddUserCalled, Is.EqualTo(wasCalled));
        }

        private void AssertWasAddBusinessCalled(bool wasCalled)
        {
            Assert.That(BusinessAddUseCase.WasAddBusinessCalled, Is.EqualTo(wasCalled));
        }

        private void AssertWasAssociateUserWithBusinessCalled(bool wasCalled)
        {
            Assert.That(AssociateUseCase.WasAssociateUserWithBusinessCalled, Is.EqualTo(wasCalled));
        }

        private void AssertPassRelevantInfoIntoAddUser()
        {
            var command = ((MockUserAddUseCase)UseCase.UserAddUseCase).Command;
            Assert.That(command, Is.Not.Null);
            Assert.That(command.FirstName, Is.EqualTo("Olaf"));
            Assert.That(command.LastName, Is.EqualTo("Thielke"));
            Assert.That(command.Email, Is.EqualTo("olaf@gmail.com"));
            Assert.That(command.Password, Is.EqualTo("password1"));
        }

        private void AssertPassRelevantInfoIntoAddBusiness()
        {
            var command = ((MockBusinessAddUseCase)UseCase.BusinessAddUseCase).Command;
            Assert.That(command, Is.Not.Null);
            Assert.That(command.Name, Is.EqualTo(BUSINESS_NAME));
            Assert.That(command.Currency, Is.EqualTo("NZD"));
        }

        private void AssertPassRelevantInfoIntoAssociateUserWithBusiness()
        {
            var command = ((MockUserAssociateWithBusinessUseCase)UseCase.UserAssociateWithBusinessUseCase).Command;
            Assert.That(command, Is.Not.Null);
            Assert.That(command.UserId, Is.EqualTo(new Guid(USER_ID)));
            Assert.That(command.BusinessId, Is.EqualTo(new Guid(BUSINESS_ID)));
            Assert.That(command.BusinessName, Is.EqualTo(BUSINESS_NAME));
        }

        private void AssertErrorResponse(IResponse response)
        {
            Assert.That(response, Is.InstanceOf<ErrorResponse>());
            var errorResponse = (ErrorResponse)response;
            Assert.That(errorResponse.IsSuccessful, Is.False);
            Assert.That(errorResponse.Errors.Count, Is.EqualTo(1));
        }

        private void AssertSuccessResponse(IResponse response)
        {
            Assert.That(response, Is.InstanceOf<Response>());
            Assert.That(response.IsSuccessful, Is.True);
            Assert.That(response.Errors, Is.Null);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data, Is.InstanceOf<RegistrationData>());
            var registration = (RegistrationData)response.Data;
            Assert.That(registration.Business, Is.Not.Null);
            Assert.That(registration.Business, Is.InstanceOf<BusinessData>());
            Assert.That(registration.Admin, Is.Not.Null);
            Assert.That(registration.Admin, Is.InstanceOf<UserData>());
        }
    }
}
