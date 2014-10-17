using CoachSeek.Application.Configuration;
using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.WebUI.Controllers;
using CoachSeek.WebUI.Tests.Unit.Fakes;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.WebUI.Tests.Unit.Controllers
{
    [TestFixture]
    public class BusinessesControllerTests
    {
        private const string BUSINESS_ID = "7028E5E1-B10E-4C0E-B46F-1386B98CE567";

        private BusinessesController Controller { get; set; }

        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            ApplicationAutoMapperConfigurator.Configure();
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


        [Test]
        public void GivenErrorOccursInUseCase_WhenGetBusinessByDomain_ThenReturnErrorResponse()
        {
            var useCase = GivenErrorOccursInUseCase();
            var response = WhenGetBusinessByDomain(useCase);
            ThenReturnErrorResponse(response);
        }

        [Test]
        public void GivenNonExistentDomain_WhenGetBusinessByDomain_ThenReturnNotFoundResponse()
        {
            var useCase = GivenNonExistentDomain();
            var response = WhenGetBusinessByDomain(useCase);
            ThenReturnNotFoundResponse(response);
        }

        [Test]
        public void GivenExistingDomain_WhenGetBusinessByDomain_ThenReturnSuccessResponse()
        {
            var useCase = GivenExistingDomain();
            var response = WhenGetBusinessByDomain(useCase);
            ThenReturnSuccessResponse(response);
        }


        private MockBusinessGetByDomainUseCase GivenErrorOccursInUseCase()
        {
            return new MockBusinessGetByDomainUseCase
            {
                Response = new Response(new ValidationException(1, "Error"))
            };
        }

        private MockBusinessGetByDomainUseCase GivenNonExistentDomain()
        {
            return new MockBusinessGetByDomainUseCase
            {
                Response = new NotFoundResponse()
            };
        }

        private MockBusinessGetByDomainUseCase GivenExistingDomain()
        {
            return new MockBusinessGetByDomainUseCase
            {
                Response = new Response(SetupBusiness())
            };
        }


        private HttpResponseMessage WhenGetBusinessByDomain(MockBusinessGetByDomainUseCase useCase)
        {
            Controller = new BusinessesController(useCase)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            return Controller.Get("davesdiscountdisaster");
        }

        private void ThenReturnErrorResponse(HttpResponseMessage response)
        {
            AssertErrorResponse(response);
            AssertPassRelevantInfoIntoGetBusinessByDomain();
        }

        private void ThenReturnNotFoundResponse(HttpResponseMessage response)
        {
            AssertNotFoundResponse(response);
            AssertPassRelevantInfoIntoGetBusinessByDomain();
        }

        private void ThenReturnSuccessResponse(HttpResponseMessage response)
        {
            AssertSuccessResponse(response);
            AssertPassRelevantInfoIntoGetBusinessByDomain();
        }

        private void AssertPassRelevantInfoIntoGetBusinessByDomain()
        {
            var domain = ((MockBusinessGetByDomainUseCase)Controller.BusinessGetByDomainUseCase).Domain;
            Assert.That(domain, Is.EqualTo("davesdiscountdisaster"));
        }

        private void AssertErrorResponse(HttpResponseMessage response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Error error;
            Assert.That(response.TryGetContentValue(out error), Is.True);
            Assert.That(error.Code, Is.EqualTo(1));
            Assert.That(error.Message, Is.EqualTo("Error"));
        }

        private void AssertNotFoundResponse(HttpResponseMessage response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Error error;
            Assert.That(response.TryGetContentValue(out error), Is.False);
            Business business;
            Assert.That(response.TryGetContentValue(out business), Is.False);
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
