using CoachSeek.Api;
using CoachSeek.Api.Controllers;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Configuration;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Tests.Unit.Fakes;
using CoachSeek.Application.UseCases;
using CoachSeek.Common;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Main.Memory.Configuration;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.WebUI.Tests.Unit.Controllers
{
    [TestFixture]
    public class LocationsControllerTests
    {
        private const string BUSINESS_ID = "12345678-B10E-4C0E-B46F-1386B98CE567";
        private const string LOCATION_ID = "90ABCDEF-42E7-4D98-8E70-4C12179DE594";

        private LocationsController Controller { get; set; }

        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            WebApiAutoMapperConfigurator.Configure();
            ApplicationAutoMapperConfigurator.Configure();
            DbAutoMapperConfigurator.Configure();
        }

        [SetUp]
        public void Setup()
        {
            Controller = new LocationsController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        private LocationData SetupLocation()
        {
            return new LocationData
            {
                Id = new Guid(LOCATION_ID),
                Name = "Orakei Tennis Club"
            };
        }


        private MockLocationAddUseCase AddUseCase
        {
            get { return (MockLocationAddUseCase)Controller.LocationAddUseCase; }
        }

        private MockLocationUpdateUseCase UpdateUseCase
        {
            get { return (MockLocationUpdateUseCase)Controller.LocationUpdateUseCase; }
        }


        [Test]
        public void WhenConstruct_ThenSetDependencies()
        {
            var controller = WhenConstruct();
            ThenSetDependencies(controller);
        }

        [Test]
        public void GivenLocationAddCommandWithError_WhenPost_ThenCallAddUseCaseAndReturnErrorResponse()
        {
            var useCase = GivenLocationAddCommandWithError();
            var response = WhenPost(useCase);
            ThenCallAddUseCaseAndReturnErrorResponse(response);
        }

        [Test]
        public void GivenLocationAddCommandWithoutError_WhenPost_ThenCallAddUseCaseAndReturnSuccessResponse()
        {
            var useCase = GivenLocationAddCommandWithoutError();
            var response = WhenPost(useCase);
            ThenCallAddUseCaseAndReturnSuccessResponse(response);
        }

        [Test]
        public void GivenLocationUpdateCommandWithError_WhenPost_ThenCallUpdateUseCaseAndReturnErrorResponse()
        {
            var useCase = GivenLocationUpdateCommandWithError();
            var response = WhenPost(useCase);
            ThenCallUpdateUseCaseAndReturnErrorResponse(response);
        }

        [Test]
        public void GivenLocationUpdateCommandWithoutError_WhenPost_ThenCallUpdateUseCaseAndReturnSuccessResponse()
        {
            var useCase = GivenLocationUpdateCommandWithoutError();
            var response = WhenPost(useCase);
            ThenCallUpdateUseCaseAndReturnSuccessResponse(response);
        }


        private MockLocationAddUseCase GivenLocationAddCommandWithError()
        {
            return new MockLocationAddUseCase
            {
                Response = new ErrorResponse(new ValidationException("Error!"))
            };
        }

        private MockLocationAddUseCase GivenLocationAddCommandWithoutError()
        {
            return new MockLocationAddUseCase
            {
                Response = new Response(SetupLocation())
            };
        }

        private MockLocationUpdateUseCase GivenLocationUpdateCommandWithError()
        {
            return new MockLocationUpdateUseCase
            {
                Response = new ErrorResponse(new ValidationException("Error!"))
            };
        }

        private MockLocationUpdateUseCase GivenLocationUpdateCommandWithoutError()
        {
            return new MockLocationUpdateUseCase
            {
                Response = new Response(SetupLocation())
            };
        }


        private LocationsController WhenConstruct()
        {
            var getAllUseCase = new LocationsGetAllUseCase();
            var getByIdUseCase = new LocationGetByIdUseCase();
            var addUseCase = new LocationAddUseCase();
            var updateUseCase = new LocationUpdateUseCase();
            var deleteUseCase = new LocationDeleteUseCase();

            return new LocationsController(getAllUseCase, getByIdUseCase, addUseCase, updateUseCase, deleteUseCase);
        }

        private HttpResponseMessage WhenPost(MockLocationAddUseCase useCase)
        {
            var apiLocationSaveCommand = new ApiLocationSaveCommand
            {
                Name = "High Street Hookers"
            };

            Controller.Business = new BusinessDetails(new Guid(BUSINESS_ID), "", "");
            Controller.LocationAddUseCase = useCase;

            return Controller.Post(apiLocationSaveCommand);
        }

        private HttpResponseMessage WhenPost(MockLocationUpdateUseCase useCase)
        {
            var apiLocationSaveCommand = new ApiLocationSaveCommand 
            { 
                Id = new Guid(LOCATION_ID),
                Name = "High Street Hookers"
            };

            Controller.Business = new BusinessDetails(new Guid(BUSINESS_ID), "", "");
            Controller.LocationUpdateUseCase = useCase;

            return Controller.Post(apiLocationSaveCommand);
        }


        private void ThenSetDependencies(LocationsController controller)
        {
            Assert.That(controller.LocationsGetAllUseCase, Is.Not.Null);
            Assert.That(controller.LocationGetByIdUseCase, Is.Not.Null);
            Assert.That(controller.LocationAddUseCase, Is.Not.Null);
            Assert.That(controller.LocationUpdateUseCase, Is.Not.Null);
            Assert.That(controller.LocationDeleteUseCase, Is.Not.Null);
        }

        private void ThenCallAddUseCaseAndReturnErrorResponse(HttpResponseMessage response)
        {
            AssertWasAddLocationCalled();
            AssertPassRelevantInfoIntoAddLocation();
            AssertErrorResponse(response);
        }

        private void ThenCallAddUseCaseAndReturnSuccessResponse(HttpResponseMessage response)
        {
            AssertWasAddLocationCalled();
            AssertPassRelevantInfoIntoAddLocation();
            AssertSuccessResponse(response);
        }

        private void ThenCallUpdateUseCaseAndReturnErrorResponse(HttpResponseMessage response)
        {
            AssertWasUpdateLocationCalled();
            AssertPassRelevantInfoIntoUpdateLocation();
            AssertErrorResponse(response);
        }

        private void ThenCallUpdateUseCaseAndReturnSuccessResponse(HttpResponseMessage response)
        {
            AssertWasUpdateLocationCalled();
            AssertPassRelevantInfoIntoUpdateLocation();
            AssertSuccessResponse(response);
        }

        private void AssertWasAddLocationCalled()
        {
            Assert.That(AddUseCase.WasAddLocationCalled, Is.True);
        }

        private void AssertWasUpdateLocationCalled()
        {
            Assert.That(UpdateUseCase.WasUpdateLocationCalled, Is.True);
        }

        private void AssertPassRelevantInfoIntoAddLocation()
        {
            var command = ((MockLocationAddUseCase)Controller.LocationAddUseCase).Command;
            Assert.That(command, Is.Not.Null);
            Assert.That(command.Name, Is.EqualTo("High Street Hookers"));
        }

        private void AssertPassRelevantInfoIntoUpdateLocation()
        {
            var command = ((MockLocationUpdateUseCase)Controller.LocationUpdateUseCase).Command;
            Assert.That(command, Is.Not.Null);

            Assert.That(command.Id, Is.EqualTo(new Guid(LOCATION_ID)));
            Assert.That(command.Name, Is.EqualTo("High Street Hookers"));
        }

        private void AssertErrorResponse(HttpResponseMessage response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            List<Error> errors;
            Assert.That(response.TryGetContentValue(out errors), Is.True);
            var error = errors[0];
            Assert.That(error.Field, Is.Null);
            Assert.That(error.Message, Is.EqualTo("Error!"));
        }

        private void AssertSuccessResponse(HttpResponseMessage response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            LocationData location;
            Assert.That(response.TryGetContentValue(out location), Is.True);
            Assert.That(location.Id, Is.EqualTo(new Guid(LOCATION_ID)));
        }
    }
}
