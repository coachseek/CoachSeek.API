using System.Collections.Generic;
using CoachSeek.Api;
using CoachSeek.Api.Controllers;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Configuration;
using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;
using CoachSeek.WebUI.Tests.Unit.Fakes;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.WebUI.Tests.Unit.Controllers
{
    [TestFixture]
    public class BusinessRegistrationControllerTests
    {
        private const string BUSINESS_ID = "12345678-B10E-4C0E-B46F-1386B98CE567";

        private BusinessRegistrationController Controller { get; set; }
        private MockUserAddUseCase UserAddUseCase { get; set; }
        private MockBusinessNewRegistrationUseCase BusinessAddUseCase { get; set; }
        private UserData UserData { get; set; }
        private BusinessData BusinessData { get; set; }

        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            WebApiAutoMapperConfigurator.Configure();
            ApplicationAutoMapperConfigurator.Configure();
        }

        [SetUp]
        public void Setup()
        {
            Controller = new BusinessRegistrationController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            UserData = new UserData
            {
                FirstName = "Olaf",
                LastName = "Thielke",
                Email = "olaf@gmail.com",
                Username = "olaf@gmail.com"
            };

            UserAddUseCase = new MockUserAddUseCase
            {
                Response = new Response<UserData>(UserData)
            };

            BusinessData = new BusinessData {Id = new Guid(BUSINESS_ID), Name = "Olaf's Cafe"};

            BusinessAddUseCase = new MockBusinessNewRegistrationUseCase
            {
                Response = new Response<BusinessData>(BusinessData)
            };
        }

        private ApiBusinessRegistrationCommand Command
        {
            get
            {
                return new ApiBusinessRegistrationCommand
                {
                    BusinessName = "Olaf's Cafe",
                    Registrant = new ApiBusinessRegistrantCommand
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
        public void GivenUserAddAndBusinessAddUseCasesSucceed_WhenPost_ThenReturnSuccessResponse()
        {
            GivenUserAddAndBusinessAddUseCasesSucceed();
            var response = WhenPost();
            ThenCallRegistrationUseCaseAndReturnSuccessResponse(response);
        }


        private void GivenUserAddUseCaseFails(string errorMessage)
        {
            UserAddUseCase.Response = new Response<UserData>(new ValidationException(errorMessage));
        }

        private void GivenBusinessAddUseCaseFails(string errorMessage)
        {
            BusinessAddUseCase.Response = new Response<BusinessData>(new ValidationException(errorMessage));
        }

        private MockBusinessNewRegistrationUseCase GivenBusinessRegistrationCommandWithError()
        {
            return new MockBusinessNewRegistrationUseCase
            {
                Response = new Response<BusinessData>(new ValidationException("My Error"))
            };
        }

        private void GivenUserAddAndBusinessAddUseCasesSucceed()
        {
        }


        private BusinessRegistrationController WhenConstruct()
        {
            var userAddUseCase = new UserAddUseCase(null);
            var registrationUseCase = new BusinessNewRegistrationUseCase(null, null, null);

            return new BusinessRegistrationController(userAddUseCase, registrationUseCase, null);
        }

        private HttpResponseMessage WhenPost()
        {
            Controller.UserAddUseCase = UserAddUseCase;
            Controller.BusinessNewRegistrationUseCase = BusinessAddUseCase;

            return Controller.Post(Command);
        }


        private void ThenSetDependencies(BusinessRegistrationController controller)
        {
            Assert.That(controller.UserAddUseCase, Is.Not.Null);
            Assert.That(controller.BusinessNewRegistrationUseCase, Is.Not.Null);
        }

        private void ThenReturnUserAddErrorResponse(HttpResponseMessage response, string errorMessage)
        {
            AssertWasAddUserCalled(true);
            AssertErrorResponse(response, errorMessage);

            AssertWasAddBusinessCalled(false);
        }

        private void ThenReturnBusinessAddErrorResponse(HttpResponseMessage response, string errorMessage)
        {
            AssertWasAddUserCalled(true);

            AssertWasAddBusinessCalled(true);
            AssertErrorResponse(response, errorMessage);
        }

        private void ThenCallRegistrationUseCaseAndReturnSuccessResponse(HttpResponseMessage response)
        {
            AssertWasAddUserCalled(true);
            AssertPassRelevantInfoIntoAddUser();

            AssertWasAddBusinessCalled(true);
            AssertPassRelevantInfoIntoRegister();

            AssertSuccessResponse(response);
        }


        private void AssertWasAddUserCalled(bool wasCalled)
        {
            Assert.That(UserAddUseCase.WasAddUserCalled, Is.EqualTo(wasCalled));
        }

        private void AssertWasAddBusinessCalled(bool wasCalled)
        {
            Assert.That(BusinessAddUseCase.WasRegisterNewBusinessCalled, Is.EqualTo(wasCalled));
        }

        private void AssertPassRelevantInfoIntoAddUser()
        {
            var command = ((MockUserAddUseCase)Controller.UserAddUseCase).Command;
            Assert.That(command, Is.Not.Null);
            Assert.That(command.FirstName, Is.EqualTo("Olaf"));
            Assert.That(command.LastName, Is.EqualTo("Thielke"));
            Assert.That(command.Email, Is.EqualTo("olaf@gmail.com"));
            Assert.That(command.Password, Is.EqualTo("password1"));
        }

        private void AssertPassRelevantInfoIntoRegister()
        {
            var command = ((MockBusinessNewRegistrationUseCase) Controller.BusinessNewRegistrationUseCase).Command;
            Assert.That(command, Is.Not.Null);
            Assert.That(command.BusinessName, Is.EqualTo("Olaf's Cafe"));
            var registrant = command.Registrant;
            Assert.That(registrant.FirstName, Is.EqualTo("Olaf"));
            Assert.That(registrant.LastName, Is.EqualTo("Thielke"));
            Assert.That(registrant.Email, Is.EqualTo("olaf@gmail.com"));
            Assert.That(registrant.Password, Is.EqualTo("password1"));
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
            BusinessData business;
            Assert.That(response.TryGetContentValue(out business), Is.True);
            Assert.That(business.Id, Is.EqualTo(new Guid(BUSINESS_ID)));
        }
    }
}
