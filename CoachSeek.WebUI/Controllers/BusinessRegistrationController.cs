using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.WebUI.Conversion;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI.Controllers
{
    public class BusinessRegistrationController : ApiController
    {
        private IBusinessNewRegistrationUseCase BusinessNewRegistrationUseCase { get; set; }

        public BusinessRegistrationController(IBusinessNewRegistrationUseCase businessNewRegistrationUseCase)
        {
            BusinessNewRegistrationUseCase = businessNewRegistrationUseCase;
        }

        // POST: api/BusinessRegistration
        public HttpResponseMessage Post([FromBody]ApiBusinessRegistrationCommand businessRegistration)
        {
            var businessAddCommand = BusinessAddCommandConverter.Convert(businessRegistration);
            var response = BusinessNewRegistrationUseCase.RegisterNewBusiness(businessAddCommand);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK, response.Business);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors[0]);
        }
    }
}
