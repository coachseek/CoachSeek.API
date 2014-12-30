using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;

namespace CoachSeek.Api.Controllers
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
        [BasicAuthentication]
        [Authorize]
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
            var command = LocationAddCommandConverter.Convert(BusinessId, location);
            var response = LocationAddUseCase.AddLocation(command);
            return CreateWebResponse(response);
        }

        private HttpResponseMessage UpdateLocation(ApiLocationSaveCommand location)
        {
            var command = LocationUpdateCommandConverter.Convert(BusinessId, location);
            var response = LocationUpdateUseCase.UpdateLocation(command);
            return CreateWebResponse(response);
        }
    }
}
