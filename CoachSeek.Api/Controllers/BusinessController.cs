using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Attributes;
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
            BusinessGetUseCase.Initialise(BusinessRepository, BusinessId);
            var response = BusinessGetUseCase.GetBusiness();
            return CreateGetWebResponse(response);
        }
    }
}
