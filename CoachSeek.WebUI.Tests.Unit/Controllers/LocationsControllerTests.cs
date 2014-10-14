using CoachSeek.Application.Contracts.Models.Responses;
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
    public class LocationsControllerTests
    {
        private const string BUSINESS_ID = "12345678-B10E-4C0E-B46F-1386B98CE567";
        private const string LOCATION_ID = "90ABCDEF-42E7-4D98-8E70-4C12179DE594";

        private LocationsController Controller { get; set; }

        [SetUp]
        public void Setup()
        {
            Controller = new LocationsController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
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
        public void GivenLocationAddRequestWithError_WhenPost_ThenCallAddUseCaseAndReturnErrorResponse()
        {
            var useCase = GivenLocationAddRequestWithError();
            var response = WhenPost(useCase);
            ThenCallAddUseCaseAndReturnErrorResponse(response);
        }

        [Test]
        public void GivenLocationAddRequestWithoutError_WhenPost_ThenCallAddUseCaseAndReturnSuccessResponse()
        {
            var useCase = GivenLocationAddRequestWithoutError();
            var response = WhenPost(useCase);
            ThenCallAddUseCaseAndReturnSuccessResponse(response);
        }

        [Test]
        public void GivenLocationUpdateRequestWithError_WhenPost_ThenCallUpdateUseCaseAndReturnErrorResponse()
        {
            var useCase = GivenLocationUpdateRequestWithError();
            var response = WhenPost(useCase);
            ThenCallUpdateUseCaseAndReturnErrorResponse(response);
        }

        [Test]
        public void GivenLocationUpdateRequestWithoutError_WhenPost_ThenCallUpdateUseCaseAndReturnSuccessResponse()
        {
            var useCase = GivenLocationUpdateRequestWithoutError();
            var response = WhenPost(useCase);
            ThenCallUpdateUseCaseAndReturnSuccessResponse(response);
        }


        private MockLocationAddUseCase GivenLocationAddRequestWithError()
        {
            return new MockLocationAddUseCase
            {
                Response = new LocationAddResponse(new ValidationException(2, "Error!"))
            };
        }

        private MockLocationAddUseCase GivenLocationAddRequestWithoutError()
        {
            return new MockLocationAddUseCase
            {
                Response = new LocationAddResponse(new Business(new Guid(BUSINESS_ID)))
            };
        }

        private MockLocationUpdateUseCase GivenLocationUpdateRequestWithError()
        {
            return new MockLocationUpdateUseCase
            {
                Response = new LocationUpdateResponse(new ValidationException(2, "Error!"))
            };
        }

        private MockLocationUpdateUseCase GivenLocationUpdateRequestWithoutError()
        {
            return new MockLocationUpdateUseCase
            {
                Response = new LocationUpdateResponse(new Business(new Guid(BUSINESS_ID)))
            };
        }


        private HttpResponseMessage WhenPost(MockLocationAddUseCase useCase)
        {
            var apiLocationSaveRequest = new ApiLocationSaveRequest {BusinessId = new Guid(BUSINESS_ID)};

            Controller.LocationAddUseCase = useCase;

            return Controller.Post(apiLocationSaveRequest);
        }

        private HttpResponseMessage WhenPost(MockLocationUpdateUseCase useCase)
        {
            var apiLocationSaveRequest = new ApiLocationSaveRequest { BusinessId = new Guid(BUSINESS_ID), Id = new Guid(LOCATION_ID) };

            Controller.LocationUpdateUseCase = useCase;

            return Controller.Post(apiLocationSaveRequest);
        }


        private void ThenCallAddUseCaseAndReturnErrorResponse(HttpResponseMessage response)
        {
            AssertWasAddLocationCalled();
            AssertErrorResponse(response);
        }

        private void ThenCallAddUseCaseAndReturnSuccessResponse(HttpResponseMessage response)
        {
            AssertWasAddLocationCalled();
            AssertSuccessResponse(response);
        }

        private void ThenCallUpdateUseCaseAndReturnErrorResponse(HttpResponseMessage response)
        {
            AssertWasUpdateLocationCalled();
            AssertErrorResponse(response);
        }

        private void ThenCallUpdateUseCaseAndReturnSuccessResponse(HttpResponseMessage response)
        {
            AssertWasUpdateLocationCalled();
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

        private void AssertErrorResponse(HttpResponseMessage response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Error error;
            Assert.That(response.TryGetContentValue(out error), Is.True);
            Assert.That(error.Code, Is.EqualTo(2));
            Assert.That(error.Message, Is.EqualTo("Error!"));
        }

        private void AssertSuccessResponse(HttpResponseMessage response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Business business;
            Assert.That(response.TryGetContentValue(out business), Is.True);
            Assert.That(business.Id, Is.EqualTo(new Guid(BUSINESS_ID)));
        }
    }
}
