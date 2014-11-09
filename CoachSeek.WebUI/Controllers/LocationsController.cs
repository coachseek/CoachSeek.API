using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.WebUI.Conversion;
using CoachSeek.WebUI.Filters;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.WebUI.Models.Api.Setup;

namespace CoachSeek.WebUI.Controllers
{
    public class LocationsController : BaseController
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
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiLocationSaveCommand location)
        {
            if (location.IsNew())
                return AddLocation(location);

            return UpdateLocation(location);
        }


        private HttpResponseMessage AddLocation(ApiLocationSaveCommand location)
        {
            var command = LocationAddCommandConverter.Convert(location);
            var response = LocationAddUseCase.AddLocation(command);
            return CreateWebResponse(response);
        }

        private HttpResponseMessage UpdateLocation(ApiLocationSaveCommand location)
        {
            var command = LocationUpdateCommandConverter.Convert(location);
            var response = LocationUpdateUseCase.UpdateLocation(command);
            return CreateWebResponse(response);
        }
    }
}
