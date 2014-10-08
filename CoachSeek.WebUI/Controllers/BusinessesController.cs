using CoachSeek.WebUI.Contracts.Persistence;
using CoachSeek.WebUI.UseCases;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.WebUI.Controllers
{
    public class BusinessesController : ApiController
    {
        private IBusinessRepository BusinessRepository { get; set; }

        public BusinessesController(IBusinessRepository businessRepository)
        {
            BusinessRepository = businessRepository;
        }

        // GET: api/Businesses/olafscafe
        [Route("api/Businesses/{domain}")]
        public HttpResponseMessage Get(string domain)
        {
            var useCase = new BusinessGetByDomainUseCase(BusinessRepository);
            var response = useCase.GetByDomain(domain);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK, response.Business);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors[0]);
        }

        // POST: api/Businesses
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Businesses/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Businesses/5
        public void Delete(int id)
        {
        }
    }
}
