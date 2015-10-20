using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Conversion.Out;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common;

namespace CoachSeek.Api.Controllers
{
    public class BusinessController : BaseController
    {
        public IBusinessGetUseCase BusinessGetUseCase { get; set; }
        public IBusinessUpdateUseCase BusinessUpdateUseCase { get; set; }

        public BusinessController(IBusinessGetUseCase businessGetUseCase, 
                                  IBusinessUpdateUseCase businessUpdateUseCase)
        {
            BusinessGetUseCase = businessGetUseCase;
            BusinessUpdateUseCase = businessUpdateUseCase;
        }


        // GET: Business
        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        public async Task<HttpResponseMessage> GetAsync()
        {
            BusinessGetUseCase.Initialise(Context);
            var business = await BusinessGetUseCase.GetBusinessAsync();
            var outBusiness = ApiOutBusinessConverter.ConvertToBusiness(business);
            return CreateGetWebResponse(outBusiness);
        }

        // POST: OnlineBooking/Business
        [Route("OnlineBooking/Business")]
        [BasicAuthenticationOrAnonymous]
        [Authorize]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<HttpResponseMessage> GetOnlineBookingAsync()
        {
            BusinessGetUseCase.Initialise(Context);
            var business = await BusinessGetUseCase.GetBusinessAsync();
            var outBasicBusiness = ApiOutBusinessConverter.ConvertToBasicBusiness(business);
            return CreateGetWebResponse(outBasicBusiness);
        }

        // POST: Business
        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<HttpResponseMessage> PostAsync([FromBody]ApiBusinessSaveCommand business)
        {
            var command = BusinessUpdateCommandConverter.Convert(business);
            BusinessUpdateUseCase.Initialise(Context);
            var response = await BusinessUpdateUseCase.UpdateBusinessAsync(command);
            return CreatePostWebResponse(response);
        }
    }
}
