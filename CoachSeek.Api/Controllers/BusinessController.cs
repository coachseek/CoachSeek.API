using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;

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
        [BusinessAuthorize]
        public async Task<HttpResponseMessage> GetAsync()
        {
            BusinessGetUseCase.Initialise(Context);
            var response = await BusinessGetUseCase.GetBusinessAsync();
            return CreateGetWebResponse(response);
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
            var response = await BusinessGetUseCase.GetBusinessAsync();
            return CreateGetWebResponse(response);
        }

        // POST: Business
        [BasicAuthentication]
        [BusinessAuthorize]
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
