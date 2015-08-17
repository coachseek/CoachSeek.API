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
        public IBusinessRegistrationUseCase BusinessRegistrationUseCase { get; set; }

        public BusinessRegistrationController(IBusinessRegistrationUseCase businessRegistrationUseCase)
        {
            BusinessRegistrationUseCase = businessRegistrationUseCase;
        }

        // POST: BusinessRegistration
        [AllowAnonymous]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiBusinessRegistrationCommand registration)
        {
            SetRepositoriesOnUseCase();
            var command = BusinessRegistrationCommandConverter.Convert(registration);
            var response = BusinessRegistrationUseCase.RegisterBusiness(command);
            return CreatePostWebResponse(response);
        }

        private void SetRepositoriesOnUseCase()
        {
            BusinessRegistrationUseCase.UserRepository = UserRepository;
            BusinessRegistrationUseCase.BusinessRepository = BusinessRepository;
            BusinessRegistrationUseCase.SupportedCurrencyRepository = SupportedCurrencyRepository;
        }
    }
}
