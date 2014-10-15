using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.WebUI.Conversion;
using CoachSeek.WebUI.Models.Api;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.WebUI.Controllers
{
    public class LocationsController : ApiController
    {
        public ILocationAddUseCase LocationAddUseCase { get; set; }
        public ILocationUpdateUseCase LocationUpdateUseCase { get; set; }

        public LocationsController()
        { }

        public LocationsController(ILocationAddUseCase locationAddUseCase,
                                   ILocationUpdateUseCase locationUpdateUseCase)
        {
            LocationAddUseCase = locationAddUseCase;
            LocationUpdateUseCase = locationUpdateUseCase;
        }


        //// GET: api/Locations
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Locations/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Locations
        public HttpResponseMessage Post([FromBody]ApiLocationSaveCommand locationSaveCommand)
        {
            if (locationSaveCommand.IsNew())
                return AddLocation(locationSaveCommand);

            return UpdateLocation(locationSaveCommand);
        }


        private HttpResponseMessage AddLocation(ApiLocationSaveCommand locationSaveCommand)
        {
            var locationAddRequest = LocationAddCommandConverter.Convert(locationSaveCommand);
            var response = LocationAddUseCase.AddLocation(locationAddRequest);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK, response.Business);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors[0]);
        }

        private HttpResponseMessage UpdateLocation(ApiLocationSaveCommand locationSaveCommand)
        {
            var locationUpdateRequest = LocationUpdateCommandConverter.Convert(locationSaveCommand);
            var response = LocationUpdateUseCase.UpdateLocation(locationUpdateRequest);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK, response.Business);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors[0]);
        }
    }
}
