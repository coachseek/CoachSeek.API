using System.Net.Http;
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
        public HttpResponseMessage Get()
        {
            BusinessGetUseCase.Initialise(Context);
            var response = BusinessGetUseCase.GetBusiness();
            return CreateGetWebResponse(response);
        }

        // POST: OnlineBooking/Business
        [Route("OnlineBooking/Business")]
        [BasicAuthenticationOrAnonymous]
        [Authorize]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage GetOnlineBooking()
        {
            BusinessGetUseCase.Initialise(Context);
            var response = BusinessGetUseCase.GetBusiness();
            return CreateGetWebResponse(response);
        }

        // POST: Business
        [BasicAuthentication]
        [BusinessAuthorize]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiBusinessSaveCommand business)
        {
            var command = BusinessUpdateCommandConverter.Convert(business);
            BusinessUpdateUseCase.Initialise(Context);
            var response = BusinessUpdateUseCase.UpdateBusiness(command);
            return CreatePostWebResponse(response);
        }
    }
}
