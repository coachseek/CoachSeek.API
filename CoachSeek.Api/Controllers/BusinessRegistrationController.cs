using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;

namespace CoachSeek.Api.Controllers
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
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiBusinessRegistrationCommand registration)
        {
            var command = BusinessAddCommandConverter.Convert(registration);
            var response = BusinessNewRegistrationUseCase.RegisterNewBusiness(command);
            return CreateWebResponse(response);
        }
    }
}
