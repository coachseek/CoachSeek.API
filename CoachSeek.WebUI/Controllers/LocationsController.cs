using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.WebUI.Conversion;
using CoachSeek.WebUI.Models.Api;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.WebUI.Controllers
{
    public class LocationsController : ApiController
    {
        private ILocationAddUseCase LocationAddUseCase { get; set; }
        private ILocationUpdateUseCase LocationUpdateUseCase { get; set; }

        public LocationsController(ILocationAddUseCase locationAddUseCase,
                                   ILocationUpdateUseCase locationUpdateUseCase)
        {
            LocationAddUseCase = locationAddUseCase;
            LocationUpdateUseCase = locationUpdateUseCase;
        }

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
        public HttpResponseMessage Post([FromBody]ApiLocationSaveRequest locationSaveRequest)
        {
            if (locationSaveRequest.IsNew())
                return AddLocation(locationSaveRequest);

            return UpdateLocation(locationSaveRequest);
        }

        private HttpResponseMessage AddLocation(ApiLocationSaveRequest locationSaveRequest)
        {
            var locationAddRequest = LocationAddRequestConverter.Convert(locationSaveRequest);
            var response = LocationAddUseCase.AddLocation(locationAddRequest);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK, response.Business);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors[0]);
        }

        private HttpResponseMessage UpdateLocation(ApiLocationSaveRequest locationSaveRequest)
        {
            var locationUpdateRequest = LocationUpdateRequestConverter.Convert(locationSaveRequest);
            var response = LocationUpdateUseCase.UpdateLocation(locationUpdateRequest);
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
