using System.Net.Http;
using System.Threading.Tasks;
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
        public async Task<HttpResponseMessage> Post([FromBody]ApiBusinessRegistrationCommand registration)
        {
            InitialiseUseCase();
            var command = BusinessRegistrationCommandConverter.Convert(registration);
            var response = await BusinessRegistrationUseCase.RegisterBusinessAsync(command);
            return CreatePostWebResponse(response);
        }

        private void InitialiseUseCase()
        {
            BusinessRegistrationUseCase.IsTesting = IsTesting;
            BusinessRegistrationUseCase.IsUserTrackingEnabled = IsUserTrackingEnabled;
            BusinessRegistrationUseCase.UserTrackerCredentials = AppSettings.UserTrackerCredentials;
            BusinessRegistrationUseCase.UserRepository = UserRepository;
            BusinessRegistrationUseCase.BusinessRepository = BusinessRepository;
            BusinessRegistrationUseCase.SupportedCurrencyRepository = SupportedCurrencyRepository;
        }
    }
}
