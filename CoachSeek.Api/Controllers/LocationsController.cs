using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common;

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
        public async Task<HttpResponseMessage> GetAsync()
        {
            LocationsGetAllUseCase.Initialise(Context);
            var response = await LocationsGetAllUseCase.GetLocationsAsync();
            return CreateGetWebResponse(response);
        }

        // GET: Locations/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthenticationOrAnonymous]
        [Authorize]
        public async Task<HttpResponseMessage> GetAsync(Guid id)
        {
            LocationGetByIdUseCase.Initialise(Context);
            var response = await LocationGetByIdUseCase.GetLocationAsync(id);
            return CreateGetWebResponse(response);
        }

        // POST: Locations
        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<HttpResponseMessage> Post([FromBody]ApiLocationSaveCommand location)
        {
            return location.IsNew() ? await AddLocationAsync(location) : await UpdateLocationAsync(location);
        }

        // DELETE: Locations/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        public async Task<HttpResponseMessage> DeleteAsync(Guid id)
        {
            LocationDeleteUseCase.Initialise(Context);
            var response = await LocationDeleteUseCase.DeleteLocationAsync(id);
            return CreateDeleteWebResponse(response);
        }

        
        private async Task<HttpResponseMessage> AddLocationAsync(ApiLocationSaveCommand location)
        {
            var command = LocationAddCommandConverter.Convert(location);
            LocationAddUseCase.Initialise(Context);
            var response = await LocationAddUseCase.AddLocationAsync(command);
            return CreatePostWebResponse(response);
        }

        private async Task<HttpResponseMessage> UpdateLocationAsync(ApiLocationSaveCommand location)
        {
            var command = LocationUpdateCommandConverter.Convert(location);
            LocationUpdateUseCase.Initialise(Context);
            var response = await LocationUpdateUseCase.UpdateLocationAsync(command);
            return CreatePostWebResponse(response);
        }
    }
}
