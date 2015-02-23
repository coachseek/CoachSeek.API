using System;
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
        public ILocationsGetAllUseCase LocationsGetAllUseCase { get; set; }
        public ILocationGetByIdUseCase LocationGetByIdUseCase { get; set; }
        public ILocationAddUseCase LocationAddUseCase { get; set; }
        public ILocationUpdateUseCase LocationUpdateUseCase { get; set; }

        public LocationsController()
        { }

        public LocationsController(ILocationsGetAllUseCase locationsGetAllUseCase,
                                   ILocationGetByIdUseCase locationGetByIdUseCase,
                                   ILocationAddUseCase locationAddUseCase,
                                   ILocationUpdateUseCase locationUpdateUseCase)
        {
            LocationsGetAllUseCase = locationsGetAllUseCase;
            LocationGetByIdUseCase = locationGetByIdUseCase;
            LocationAddUseCase = locationAddUseCase;
            LocationUpdateUseCase = locationUpdateUseCase;
        }


        // GET: api/Locations
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get()
        {
            LocationsGetAllUseCase.BusinessId = BusinessId;
            var response = LocationsGetAllUseCase.GetLocations();
            return CreateGetWebResponse(response);
        }

        // GET: api/Locations/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            LocationGetByIdUseCase.BusinessId = BusinessId;
            var response = LocationGetByIdUseCase.GetLocation(id);
            return CreateGetWebResponse(response);
        }

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
            var command = LocationAddCommandConverter.Convert(location);
            LocationAddUseCase.BusinessId = BusinessId;
            var response = LocationAddUseCase.AddLocation(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateLocation(ApiLocationSaveCommand location)
        {
            var command = LocationUpdateCommandConverter.Convert(location);
            LocationUpdateUseCase.BusinessId = BusinessId; 
            var response = LocationUpdateUseCase.UpdateLocation(command);
            return CreatePostWebResponse(response);
        }
    }
}
