using CoachSeek.WebUI.Contracts.UseCases;
using CoachSeek.WebUI.Models.Requests;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
        public HttpResponseMessage Post([FromBody]BusinessRegistrationRequest businessRegistration)
        {
            var response = BusinessNewRegistrationUseCase.RegisterNewBusiness(businessRegistration);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK, response.Business);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors[0]);
        }
    }
}
