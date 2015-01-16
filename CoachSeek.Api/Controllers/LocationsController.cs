using System;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Api.Controllers
{
    public class LocationsController : BaseController
    {
        public ILocationGetUseCase LocationGetUseCase { get; set; }
        public ILocationAddUseCase LocationAddUseCase { get; set; }
        public ILocationUpdateUseCase LocationUpdateUseCase { get; set; }

        public LocationsController()
        { }

        public LocationsController(ILocationGetUseCase locationGetUseCase,
                                   ILocationAddUseCase locationAddUseCase,
                                   ILocationUpdateUseCase locationUpdateUseCase)
        {
            LocationGetUseCase = locationGetUseCase;
            LocationAddUseCase = locationAddUseCase;
            LocationUpdateUseCase = locationUpdateUseCase;
        }


        //// GET: api/Locations
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/Locations/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            LocationGetUseCase.BusinessId = BusinessId;
            var response = LocationGetUseCase.GetLocation(id);
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
            var command = LocationAddCommandConverter.Convert(BusinessId, location);
            var response = LocationAddUseCase.AddLocation(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateLocation(ApiLocationSaveCommand location)
        {
            var command = LocationUpdateCommandConverter.Convert(BusinessId, location);
            var response = LocationUpdateUseCase.UpdateLocation(command);
            return CreatePostWebResponse(response);
        }
    }
}
