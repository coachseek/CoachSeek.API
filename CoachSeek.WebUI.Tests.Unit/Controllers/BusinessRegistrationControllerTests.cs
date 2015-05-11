using CoachSeek.Api;
using CoachSeek.Api.Controllers;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Configuration;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Tests.Unit.Fakes;
using CoachSeek.Application.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Services;
using CoachSeek.WebUI.Tests.Unit.Fakes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.WebUI.Tests.Unit.Controllers
{
    [TestFixture]
    public class BusinessRegistrationControllerTests
    {
        private const string USER_ID = "87654321-F027-990A-EDF0-ADED12F4880B";
        private const string BUSINESS_ID = "12345678-B10E-4C0E-B46F-1386B98CE567";
        private const string BUSINESS_NAME = "Olaf's Scuba Shoppe";
        private const string BUSINESS_DOMAIN = "olafsscubashoppe";

        private BusinessRegistrationController Controller { get; set; }
        private MockUserAddUseCase UserAddUseCase { get; set; }
        private MockBusinessAddUseCase BusinessAddUseCase { get; set; }
        private MockUserAssociateWithBusinessUseCase AssociateUseCase { get; set; }
        private UserData UserData { get; set; }
        private BusinessData BusinessData { get; set; }

        //private StubBusinessRegistrationEmailer Emailer
        //{
        //    get { return ((StubBusinessRegistrationEmailer)Controller.BusinessRegistrationEmailer); }
        //}

        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            WebApiAutoMapperConfigurator.Configure();
            ApplicationAutoMapperConfigurator.Configure();
        }

        [SetUp]
        public void Setup()
        {
            SetupController();
            SetupUserAddUseCase();
            SetupBusinessAddUseCase();
            SetupAssociateUseCase();
        }

        private void SetupController()
        {
            Controller = new BusinessRegistrationController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
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

        private ApiBusinessRegistrationCommand Command
        {
            get
            {
                return new ApiBusinessRegistrationCommand
                {
                    Business = new ApiBusinessCommand
                    {
                        Name = BUSINESS_NAME
                    },
                    Admin = new ApiBusinessAdminCommand
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
        public void WhenConstruct_ThenSetDependencies()
        {
            var controller = WhenConstruct();
            ThenSetDependencies(controller);
        }

        [Test]
        public void GivenUserAddUseCaseFails_WhenPost_ThenReturnErrorResponse()
        {
            GivenUserAddUseCaseFails("An error has occurred!");
            var response = WhenPost();
            ThenReturnUserAddErrorResponse(response, "An error has occurred!");
        }

        [Test]
        public void GivenBusinessAddUseCaseFails_WhenPost_ThenReturnErrorResponse()
        {
            GivenBusinessAddUseCaseFails("An error has occurred!");
            var response = WhenPost();
            ThenReturnBusinessAddErrorResponse(response, "An error has occurred!");
        }

        [Test]
        public void GivenAssociateUseCaseFails_WhenPost_ThenReturnErrorResponse()
        {
            GivenAssociateUseCaseFails("An error has occurred!");
            var response = WhenPost();
            ThenReturnAssociateErrorResponse(response, "An error has occurred!");
        }

        [Test]
        public void GivenAllUseCasesSucceed_WhenPost_ThenReturnSuccessResponse()
        {
            GivenAllUseCasesSucceed();
            var response = WhenPost();
            ThenReturnSuccessResponse(response);
        }


        private void GivenUserAddUseCaseFails(string errorMessage)
        {
            UserAddUseCase.Response = new ErrorResponse(new ValidationException(errorMessage));
        }

        private void GivenBusinessAddUseCaseFails(string errorMessage)
        {
            BusinessAddUseCase.Response = new ErrorResponse(new ValidationException(errorMessage));
        }

        private void GivenAssociateUseCaseFails(string errorMessage)
        {
            AssociateUseCase.Response = new ErrorResponse(new ValidationException(errorMessage));
        }

        private void GivenAllUseCasesSucceed()
        {
            // Already all set up.
        }


        private BusinessRegistrationController WhenConstruct()
        {
            var userAddUseCase = new UserAddUseCase();
            var businessAddUseCase = new BusinessAddUseCase(null);
            var associateUseCase = new UserAssociateWithBusinessUseCase();

            return new BusinessRegistrationController(userAddUseCase, businessAddUseCase, associateUseCase);
        }

        private HttpResponseMessage WhenPost()
        {
            Controller.UserAddUseCase = UserAddUseCase;
            Controller.BusinessAddUseCase = BusinessAddUseCase;
            Controller.UserAssociateWithBusinessUseCase = AssociateUseCase;

            return Controller.Post(Command);
        }


        private void ThenSetDependencies(BusinessRegistrationController controller)
        {
            Assert.That(controller.UserAddUseCase, Is.Not.Null);
            Assert.That(controller.BusinessAddUseCase, Is.Not.Null);
            Assert.That(controller.UserAssociateWithBusinessUseCase, Is.Not.Null);
        }

        private void ThenReturnUserAddErrorResponse(HttpResponseMessage response, string errorMessage)
        {
            AssertWasAddUserCalled(true);
            AssertPassRelevantInfoIntoAddUser();

            AssertWasAddBusinessCalled(false);
            AssertWasAssociateUserWithBusinessCalled(false);
            //AssertWasSendEmailCalled(false);

            AssertErrorResponse(response, errorMessage);
        }

        private void ThenReturnBusinessAddErrorResponse(HttpResponseMessage response, string errorMessage)
        {
            AssertWasAddUserCalled(true);
            AssertPassRelevantInfoIntoAddUser();

            AssertWasAddBusinessCalled(true);
            AssertPassRelevantInfoIntoAddBusiness();

            AssertWasAssociateUserWithBusinessCalled(false);
            //AssertWasSendEmailCalled(false);

            AssertErrorResponse(response, errorMessage);
        }

        private void ThenReturnAssociateErrorResponse(HttpResponseMessage response, string errorMessage)
        {
            AssertWasAddUserCalled(true);
            AssertPassRelevantInfoIntoAddUser();

            AssertWasAddBusinessCalled(true);
            AssertPassRelevantInfoIntoAddBusiness();

            AssertWasAssociateUserWithBusinessCalled(true);
            AssertPassRelevantInfoIntoAssociateUserWithBusiness();

            //AssertWasSendEmailCalled(false);

            AssertErrorResponse(response, errorMessage);
        }

        private void ThenReturnSuccessResponse(HttpResponseMessage response)
        {
            AssertWasAddUserCalled(true);
            AssertPassRelevantInfoIntoAddUser();

            AssertWasAddBusinessCalled(true);
            AssertPassRelevantInfoIntoAddBusiness();

            AssertWasAssociateUserWithBusinessCalled(true);
            AssertPassRelevantInfoIntoAssociateUserWithBusiness();

            //AssertWasSendEmailCalled(true);

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

        //private void AssertWasSendEmailCalled(bool wasCalled)
        //{
        //    Assert.That(Emailer.WasSendEmailCalled, Is.EqualTo(wasCalled));
        //}

        private void AssertPassRelevantInfoIntoAddUser()
        {
            var command = ((MockUserAddUseCase)Controller.UserAddUseCase).Command;
            Assert.That(command, Is.Not.Null);
            Assert.That(command.FirstName, Is.EqualTo("Olaf"));
            Assert.That(command.LastName, Is.EqualTo("Thielke"));
            Assert.That(command.Email, Is.EqualTo("olaf@gmail.com"));
            Assert.That(command.Password, Is.EqualTo("password1"));
        }

        private void AssertPassRelevantInfoIntoAddBusiness()
        {
            var command = ((MockBusinessAddUseCase)Controller.BusinessAddUseCase).Command;
            Assert.That(command, Is.Not.Null);
            Assert.That(command.Name, Is.EqualTo(BUSINESS_NAME));
        }

        private void AssertPassRelevantInfoIntoAssociateUserWithBusiness()
        {
            var command = ((MockUserAssociateWithBusinessUseCase)Controller.UserAssociateWithBusinessUseCase).Command;
            Assert.That(command, Is.Not.Null);
            Assert.That(command.UserId, Is.EqualTo(new Guid(USER_ID)));
            Assert.That(command.BusinessId, Is.EqualTo(new Guid(BUSINESS_ID)));
            Assert.That(command.BusinessName, Is.EqualTo(BUSINESS_NAME));
        }

        private void AssertErrorResponse(HttpResponseMessage response, string errorMessage)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            List<ErrorData> errors;
            Assert.That(response.TryGetContentValue(out errors), Is.True);
            var error = errors[0];
            Assert.That(error.Field, Is.Null);
            Assert.That(error.Message, Is.EqualTo(errorMessage));
        }

        private void AssertSuccessResponse(HttpResponseMessage response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            RegistrationData registration;
            Assert.That(response.TryGetContentValue(out registration), Is.True);
            Assert.That(registration.Business, Is.Not.Null);
            Assert.That(registration.Business, Is.InstanceOf<BusinessData>());
            var business = registration.Business;
            Assert.That(business.Id, Is.EqualTo(new Guid(BUSINESS_ID)));
            Assert.That(business.Name, Is.EqualTo(BUSINESS_NAME));
            Assert.That(business.Domain, Is.EqualTo(BUSINESS_DOMAIN));
            Assert.That(registration.Admin, Is.Not.Null);
            Assert.That(registration.Admin, Is.InstanceOf<UserData>());
            var admin = registration.Admin;
            Assert.That(admin.Id, Is.EqualTo(new Guid(USER_ID)));
            Assert.That(admin.FirstName, Is.EqualTo("Olaf"));
            Assert.That(admin.LastName, Is.EqualTo("Thielke"));
            Assert.That(admin.Email, Is.EqualTo("olaf@gmail.com"));
            Assert.That(admin.Username, Is.EqualTo("olaf@gmail.com"));
        }
    }
}
