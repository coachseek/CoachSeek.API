using System.Collections.Generic;
using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.UseCases;
using CoachSeek.Data.Model;
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
    public class LocationsControllerTests
    {
        private const string BUSINESS_ID = "12345678-B10E-4C0E-B46F-1386B98CE567";
        private const string LOCATION_ID = "90ABCDEF-42E7-4D98-8E70-4C12179DE594";

        private LocationsController Controller { get; set; }

        [SetUp]
        public void Setup()
        {
            Controller = new LocationsController()
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

        //private Business SetupBusiness()
        //{
        //    return new Business(new Guid(BUSINESS_ID),
        //        "Olaf's Cafe",
        //        "olafscafe",
        //        new BusinessAdminData
        //        {
        //            FirstName = "Bobby",
        //            LastName = "Tables",
        //            Email = "bobby@tables.hack",
        //            Username = "bobby@tables.hack",
        //        },
        //        null,
        //        null);
        //}


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
                Response = new Response<LocationData>(new ValidationException("Error!"))
            };
        }

        private MockLocationAddUseCase GivenLocationAddCommandWithoutError()
        {
            return new MockLocationAddUseCase
            {
                Response = new Response<LocationData>(SetupLocation())
            };
        }

        private MockLocationUpdateUseCase GivenLocationUpdateCommandWithError()
        {
            return new MockLocationUpdateUseCase
            {
                Response = new Response<LocationData>(new ValidationException("Error!"))
            };
        }

        private MockLocationUpdateUseCase GivenLocationUpdateCommandWithoutError()
        {
            return new MockLocationUpdateUseCase
            {
                Response = new Response<LocationData>(SetupLocation())
            };
        }


        private LocationsController WhenConstruct()
        {
            var addUseCase = new LocationAddUseCase(null);
            var updateUseCase = new LocationUpdateUseCase(null);

            return new LocationsController(addUseCase, updateUseCase);
        }

        private HttpResponseMessage WhenPost(MockLocationAddUseCase useCase)
        {
            var apiLocationSaveCommand = new ApiLocationSaveCommand
            {
                BusinessId = new Guid(BUSINESS_ID),
                Name = "High Street Hookers"
            };

            Controller.LocationAddUseCase = useCase;

            return Controller.Post(apiLocationSaveCommand);
        }

        private HttpResponseMessage WhenPost(MockLocationUpdateUseCase useCase)
        {
            var apiLocationSaveCommand = new ApiLocationSaveCommand 
            { 
                BusinessId = new Guid(BUSINESS_ID), 
                Id = new Guid(LOCATION_ID),
                Name = "High Street Hookers"
            };

            Controller.LocationUpdateUseCase = useCase;

            return Controller.Post(apiLocationSaveCommand);
        }


        private void ThenSetDependencies(LocationsController controller)
        {
            Assert.That(controller.LocationAddUseCase, Is.Not.Null);
            Assert.That(controller.LocationUpdateUseCase, Is.Not.Null);
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
            Assert.That(command.BusinessId, Is.EqualTo(new Guid(BUSINESS_ID)));
            Assert.That(command.LocationName, Is.EqualTo("High Street Hookers"));
        }

        private void AssertPassRelevantInfoIntoUpdateLocation()
        {
            var command = ((MockLocationUpdateUseCase)Controller.LocationUpdateUseCase).Command;
            Assert.That(command, Is.Not.Null);
            Assert.That(command.BusinessId, Is.EqualTo(new Guid(BUSINESS_ID)));
            Assert.That(command.LocationId, Is.EqualTo(new Guid(LOCATION_ID)));
            Assert.That(command.LocationName, Is.EqualTo("High Street Hookers"));
        }

        private void AssertErrorResponse(HttpResponseMessage response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            List<ErrorData> errors;
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
