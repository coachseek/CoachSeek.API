using CoachSeek.WebUI.Models.Requests;
using CoachSeek.WebUI.Persistence;
using CoachSeek.WebUI.UseCases;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.WebUI.Controllers
{
    public class LocationsController : ApiController
    {
        // GET: api/Locations
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Locations/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Locations
        public HttpResponseMessage Post([FromBody]LocationAddRequest locationAddRequest)
        {
            var businessRepository = new InMemoryBusinessRepository();

            var useCase = new LocationAddUseCase(businessRepository);
            var response = useCase.AddLocation(locationAddRequest);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK, response.Business);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors[0]);
        }

        // PUT: api/Locations/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Locations/5
        public void Delete(int id)
        {
        }
    }
}
