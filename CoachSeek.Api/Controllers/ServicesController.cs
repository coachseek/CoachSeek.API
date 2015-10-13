using System.Threading.Tasks;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;
using System;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.Common;

namespace CoachSeek.Api.Controllers
{
    public class ServicesController : BaseController
    {
        public IServicesGetAllUseCase ServicesGetAllUseCase { get; set; }
        public IServiceGetByIdUseCase ServiceGetByIdUseCase { get; set; }
        public IServiceAddUseCase ServiceAddUseCase { get; set; }
        public IServiceUpdateUseCase ServiceUpdateUseCase { get; set; }
        public IServiceDeleteUseCase ServiceDeleteUseCase { get; set; }

        public ServicesController()
        { }

        public ServicesController(IServicesGetAllUseCase servicesGetAllUseCase,
                                  IServiceGetByIdUseCase serviceGetByIdUseCase,
                                  IServiceAddUseCase serviceAddUseCase, 
                                  IServiceUpdateUseCase serviceUpdateUseCase,
                                  IServiceDeleteUseCase serviceDeleteUseCase)
        {
            ServicesGetAllUseCase = servicesGetAllUseCase;
            ServiceGetByIdUseCase = serviceGetByIdUseCase;
            ServiceAddUseCase = serviceAddUseCase;
            ServiceUpdateUseCase = serviceUpdateUseCase;
            ServiceDeleteUseCase = serviceDeleteUseCase;
        }


        // GET: Services
        [BasicAuthenticationOrAnonymous]
        [Authorize]
        public async Task<HttpResponseMessage> GetAsync()
        {
            ServicesGetAllUseCase.Initialise(Context);
            var response = await ServicesGetAllUseCase.GetServicesAsync();
            return CreateGetWebResponse(response);
        }

        // GET: Services/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthenticationOrAnonymous]
        [Authorize]
        public async Task<HttpResponseMessage> GetAsync(Guid id)
        {
            ServiceGetByIdUseCase.Initialise(Context);
            var response = await ServiceGetByIdUseCase.GetServiceAsync(id);
            return CreateGetWebResponse(response);
        }

        // POST: Services
        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiServiceSaveCommand service)
        {
            if (service.IsNew())
                return AddService(service);

            return UpdateService(service);
        }

        // DELETE: Services/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        public HttpResponseMessage Delete(Guid id)
        {
            ServiceDeleteUseCase.Initialise(Context);
            var response = ServiceDeleteUseCase.DeleteService(id);
            return CreateDeleteWebResponse(response);
        }


        private HttpResponseMessage AddService(ApiServiceSaveCommand service)
        {
            var command = ServiceAddCommandConverter.Convert(service);
            ServiceAddUseCase.Initialise(Context);
            var response = ServiceAddUseCase.AddService(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateService(ApiServiceSaveCommand service)
        {
            var command = ServiceUpdateCommandConverter.Convert(service);
            ServiceUpdateUseCase.Initialise(Context);
            var response = ServiceUpdateUseCase.UpdateService(command);
            return CreatePostWebResponse(response);
        }
    }
}
