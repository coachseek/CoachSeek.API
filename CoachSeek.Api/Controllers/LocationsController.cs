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
        public ILocationDeleteUseCase LocationDeleteUseCase { get; set; }

        public LocationsController()
        { }

        public LocationsController(ILocationsGetAllUseCase locationsGetAllUseCase,
                                   ILocationGetByIdUseCase locationGetByIdUseCase,
                                   ILocationAddUseCase locationAddUseCase,
                                   ILocationUpdateUseCase locationUpdateUseCase,
                                   ILocationDeleteUseCase locationDeleteUseCase)
        {
            LocationsGetAllUseCase = locationsGetAllUseCase;
            LocationGetByIdUseCase = locationGetByIdUseCase;
            LocationAddUseCase = locationAddUseCase;
            LocationUpdateUseCase = locationUpdateUseCase;
            LocationDeleteUseCase = locationDeleteUseCase;
        }


        // GET: api/Locations
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get()
        {
            LocationsGetAllUseCase.Initialise(BusinessRepository, BusinessId);
            var response = LocationsGetAllUseCase.GetLocations();
            return CreateGetWebResponse(response);
        }

        // GET: api/Locations/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            LocationGetByIdUseCase.Initialise(BusinessRepository, BusinessId);
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

        // DELETE: api/Locations/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Delete(Guid id)
        {
            LocationDeleteUseCase.Initialise(BusinessRepository, BusinessId);
            var response = LocationDeleteUseCase.DeleteLocation(id);
            return CreateDeleteWebResponse(response);
        }

        
        private HttpResponseMessage AddLocation(ApiLocationSaveCommand location)
        {
            var command = LocationAddCommandConverter.Convert(location);
            LocationAddUseCase.Initialise(BusinessRepository, BusinessId);
            var response = LocationAddUseCase.AddLocation(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateLocation(ApiLocationSaveCommand location)
        {
            var command = LocationUpdateCommandConverter.Convert(location);
            LocationUpdateUseCase.Initialise(BusinessRepository, BusinessId);
            var response = LocationUpdateUseCase.UpdateLocation(command);
            return CreatePostWebResponse(response);
        }
    }
}
