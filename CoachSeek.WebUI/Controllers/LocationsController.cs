using CoachSeek.WebUI.Contracts.Persistence;
using CoachSeek.WebUI.Conversion;
using CoachSeek.WebUI.Models.Api;
using CoachSeek.WebUI.UseCases;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.WebUI.Controllers
{
    public class LocationsController : ApiController
    {
        private IBusinessRepository BusinessRepository { get; set; }

        public LocationsController(IBusinessRepository businessRepository)
        {
            BusinessRepository = businessRepository;
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
            var useCase = new LocationAddUseCase(BusinessRepository);
            var response = useCase.AddLocation(locationAddRequest);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK, response.Business);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors[0]);
        }

        private HttpResponseMessage UpdateLocation(ApiLocationSaveRequest locationSaveRequest)
        {
            var locationUpdateRequest = LocationUpdateRequestConverter.Convert(locationSaveRequest);
            var updateUseCase = new LocationUpdateUseCase(BusinessRepository);
            var response = updateUseCase.UpdateLocation(locationUpdateRequest);
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
