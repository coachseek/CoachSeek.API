using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.WebUI.Models.Requests;
using CoachSeek.WebUI.UseCases;

namespace CoachSeek.WebUI.Controllers
{
    public class LocationsController : ApiController
    {
        // GET: api/BusinessLocations
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/BusinessLocations/5
        public string Get(int id)
        {
            return "value";
        }

        //// POST: api/BusinessRegistration
        //public HttpResponseMessage Post([FromBody]LocationAddRequest locationAddRequest)
        //{
        //    var useCase = new LocationAddUseCase(businessRepository, businessAdminRepository, domainBuilder, emailer);

        //    var response = useCase.RegisterNewBusiness(businessRegistration);
        //    if (response.IsSuccessful)
        //        return Request.CreateResponse(HttpStatusCode.OK, response.Business);
        //    return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors[0]);
        //}

        // PUT: api/BusinessLocations/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/BusinessLocations/5
        public void Delete(int id)
        {
        }
    }
}
