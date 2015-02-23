using System.Net.Http;
using System.Web.Http;
using CoachSeek.Application.Contracts.UseCases;

namespace CoachSeek.Api.Controllers
{
    public class BusinessesController : BaseController
    {
        //public IBusinessGetByDomainUseCase BusinessGetByDomainUseCase { get; set; }

        //public BusinessesController(IBusinessGetByDomainUseCase businessGetByDomainUseCase)
        //{
        //    BusinessGetByDomainUseCase = businessGetByDomainUseCase;
        //}

        //// GET: api/Businesses/olafscafe
        //[Route("api/Businesses/{domain}")]
        //public HttpResponseMessage Get(string domain)
        //{
        //    var response = BusinessGetByDomainUseCase.GetByDomain(domain);
        //    return CreateGetWebResponse(response);
        //}
    }
}
