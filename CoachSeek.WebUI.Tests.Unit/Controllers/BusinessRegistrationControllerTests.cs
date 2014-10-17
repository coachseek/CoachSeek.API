using CoachSeek.Application.Configuration;
using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.WebUI.Controllers;
using CoachSeek.WebUI.Models.Api;
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

        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            WebApiAutoMapperConfigurator.Configure();
            ApplicationAutoMapperConfigurator.Configure();
        }

        [SetUp]
        public void Setup()
        {
            Controller = new BusinessRegistrationController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        private Business SetupBusiness()
        {
            return new Business(new Guid(BUSINESS_ID),
                "Olaf's Cafe",
                "olafscafe",
                new BusinessAdminData
                {
                    FirstName = "Bobby",
                    LastName = "Tables",
                    Email = "bobby@tables.hack",
                    Username = "bobby@tables.hack",
                },
                null,
                null);
        }

        private MockBusinessNewRegistrationUseCase RegisterUseCase
        {
            get { return (MockBusinessNewRegistrationUseCase)Controller.BusinessNewRegistrationUseCase; }
        }


        [Test]
        public void WhenConstruct_ThenSetDependencies()
        {
            var controller = WhenConstruct();
            ThenSetDependencies(controller);
        }

        [Test]
        public void GivenBusinessRegistrationCommandWithError_WhenPost_ThenCallRegistrationUseCaseAndReturnErrorResponse()
        {
            var useCase = GivenBusinessRegistrationCommandWithError();
            var response = WhenPost(useCase);
            ThenCallRegistrationUseCaseAndReturnErrorResponse(response);
        }

        [Test]
        public void GivenBusinessRegistrationCommandWithoutError_WhenPost_ThenCallRegistrationUseCaseAndReturnSuccessResponse()
        {
            var useCase = GivenBusinessRegistrationCommandWithoutError();
            var response = WhenPost(useCase);
            ThenCallRegistrationUseCaseAndReturnSuccessResponse(response);
        }


        private MockBusinessNewRegistrationUseCase GivenBusinessRegistrationCommandWithError()
        {
            return new MockBusinessNewRegistrationUseCase
            {
                Response = new Response(new ValidationException(3, "My Error"))
            };
        }

        private MockBusinessNewRegistrationUseCase GivenBusinessRegistrationCommandWithoutError()
        {
            return new MockBusinessNewRegistrationUseCase
            {
                Response = new Response(SetupBusiness())
            };
        }


        private BusinessRegistrationController WhenConstruct()
        {
            var registrationUseCase = new BusinessNewRegistrationUseCase(null, null, null);

            return new BusinessRegistrationController(registrationUseCase);
        }

        private HttpResponseMessage WhenPost(MockBusinessNewRegistrationUseCase useCase)
        {
            var apiBusinessRegistrationCommand = new ApiBusinessRegistrationCommand
            {
                BusinessName = "Olaf's Cafe",
                Registrant = new ApiBusinessRegistrant
                {
                    FirstName = "Olaf",
                    LastName = "Thielke",
                    Email = "olaf@gmail.com",
                    Password = "password1"
                }
            };

            Controller.BusinessNewRegistrationUseCase = useCase;

            return Controller.Post(apiBusinessRegistrationCommand);
        }


        private void ThenSetDependencies(BusinessRegistrationController controller)
        {
            Assert.That(controller.BusinessNewRegistrationUseCase, Is.Not.Null);
        }

        private void ThenCallRegistrationUseCaseAndReturnErrorResponse(HttpResponseMessage response)
        {
            AssertWasRegisterNewBusinessCalledCalled();
            AssertPassRelevantInfoIntoRegister();
            AssertErrorResponse(response);
        }

        private void ThenCallRegistrationUseCaseAndReturnSuccessResponse(HttpResponseMessage response)
        {
            AssertWasRegisterNewBusinessCalledCalled();
            AssertPassRelevantInfoIntoRegister();
            AssertSuccessResponse(response);
        }

        private void AssertWasRegisterNewBusinessCalledCalled()
        {
            Assert.That(RegisterUseCase.WasRegisterNewBusinessCalled, Is.True);
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

        private void AssertErrorResponse(HttpResponseMessage response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Error error;
            Assert.That(response.TryGetContentValue(out error), Is.True);
            Assert.That(error.Code, Is.EqualTo(3));
            Assert.That(error.Message, Is.EqualTo("My Error"));
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
