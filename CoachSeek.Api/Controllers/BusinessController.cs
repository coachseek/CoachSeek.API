using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Booking;
using CoachSeek.Application.Contracts.UseCases;

namespace CoachSeek.Api.Controllers
{
    public class BusinessController : BaseController
    {
        public IBusinessGetUseCase BusinessGetUseCase { get; set; }

        public BusinessController(IBusinessGetUseCase businessGetUseCase)
        {
            BusinessGetUseCase = businessGetUseCase;
        }


        // GET: Business
        [BasicAuthentication]
        [Authorize]
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
    }
}
