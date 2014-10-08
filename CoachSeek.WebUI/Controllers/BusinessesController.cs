using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.WebUI.Conversion;
using CoachSeek.WebUI.Persistence;
using CoachSeek.WebUI.UseCases;

namespace CoachSeek.WebUI.Controllers
{
    public class BusinessesController : ApiController
    {
        //// GET: api/Businesses
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/Businesses/olafscafe
        [Route("api/Businesses/{domain}")]
        public HttpResponseMessage Get(string domain)
        {
            var businessRepository = new InMemoryBusinessRepository();

            var useCase = new BusinessGetByDomainUseCase(businessRepository);
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
