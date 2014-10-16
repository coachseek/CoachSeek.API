using System.Net.Http;
using System.Web.Http;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.WebUI.Conversion;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI.Controllers
{
    public class BusinessRegistrationController : BaseController
    {
        public IBusinessNewRegistrationUseCase BusinessNewRegistrationUseCase { get; set; }

        public BusinessRegistrationController()
        { }

        public BusinessRegistrationController(IBusinessNewRegistrationUseCase businessNewRegistrationUseCase)
        {
            BusinessNewRegistrationUseCase = businessNewRegistrationUseCase;
        }


        // POST: api/BusinessRegistration
        public HttpResponseMessage Post([FromBody]ApiBusinessRegistrationCommand registration)
        {
            var command = BusinessAddCommandConverter.Convert(registration);
            var response = BusinessNewRegistrationUseCase.RegisterNewBusiness(command);
            return CreateWebResponse(response);
        }
    }
}
