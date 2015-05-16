using System;
using System.Net;
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


        // GET: Locations
        [BasicAuthenticationOrAnonymous]
        [Authorize]
        public HttpResponseMessage Get()
        {
            LocationsGetAllUseCase.Initialise(Context);
            var response = LocationsGetAllUseCase.GetLocations();
            return CreateGetWebResponse(response);
        }

        //// GET: OnlineBooking/Locations
        //[Route("OnlineBooking/Locations")]
        //[BasicAuthenticationOrAnonymous]
        //[Authorize]
        //public HttpResponseMessage GetForOnlineBooking()
        //{
        //    LocationsGetAllUseCase.Initialise(BusinessRepository, BusinessId);
        //    var response = LocationsGetAllUseCase.GetLocations();
        //    return CreateGetWebResponse(response);
        //}

        // GET: Locations/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthenticationOrAnonymous]
        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            // TODO: Remove! Only here for testing unhandled exception handling.
            if (id == Guid.Empty)
                throw new InvalidOperationException("testing");
            // TODO: Remove!

            LocationGetByIdUseCase.Initialise(Context);
            var response = LocationGetByIdUseCase.GetLocation(id);
            return CreateGetWebResponse(response);
        }

        // POST: Locations
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

        // DELETE: Locations/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Delete(Guid id)
        {
            LocationDeleteUseCase.Initialise(Context);
            var response = LocationDeleteUseCase.DeleteLocation(id);
            return CreateDeleteWebResponse(response);
        }

        
        private HttpResponseMessage AddLocation(ApiLocationSaveCommand location)
        {
            var command = LocationAddCommandConverter.Convert(location);
            LocationAddUseCase.Initialise(Context);
            var response = LocationAddUseCase.AddLocation(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateLocation(ApiLocationSaveCommand location)
        {
            var command = LocationUpdateCommandConverter.Convert(location);
            LocationUpdateUseCase.Initialise(Context);
            var response = LocationUpdateUseCase.UpdateLocation(command);
            return CreatePostWebResponse(response);
        }
    }
}
