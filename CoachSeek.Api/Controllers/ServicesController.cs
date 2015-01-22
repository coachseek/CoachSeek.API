using System;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.UseCases;

namespace CoachSeek.Api.Controllers
{
    public class ServicesController : BaseController
    {
        public IServicesGetAllUseCase ServicesGetAllUseCase { get; set; }
        public IServiceGetByIdUseCase ServiceGetByIdUseCase { get; set; }
        public IServiceAddUseCase ServiceAddUseCase { get; set; }
        public IServiceUpdateUseCase ServiceUpdateUseCase { get; set; }

        public ServicesController()
        { }

        public ServicesController(IServicesGetAllUseCase servicesGetAllUseCase,
                                  IServiceGetByIdUseCase serviceGetByIdUseCase,
                                  IServiceAddUseCase serviceAddUseCase, 
                                  IServiceUpdateUseCase serviceUpdateUseCase)
        {
            ServicesGetAllUseCase = servicesGetAllUseCase;
            ServiceGetByIdUseCase = serviceGetByIdUseCase;
            ServiceAddUseCase = serviceAddUseCase;
            ServiceUpdateUseCase = serviceUpdateUseCase;
        }


        // GET: api/Services
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get()
        {
            ServicesGetAllUseCase.BusinessId = BusinessId;
            var response = ServicesGetAllUseCase.GetServices();
            return CreateGetWebResponse(response);
        }

        // GET: api/Services/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            ServiceGetByIdUseCase.BusinessId = BusinessId;
            var response = ServiceGetByIdUseCase.GetService(id);
            return CreateGetWebResponse(response);
        }

        // POST: api/Services
        [BasicAuthentication]
        [Authorize]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiServiceSaveCommand service)
        {
            if (service.IsNew())
                return AddService(service);

            return UpdateService(service);
        }


        private HttpResponseMessage AddService(ApiServiceSaveCommand service)
        {
            var command = ServiceAddCommandConverter.Convert(BusinessId, service);
            var response = ServiceAddUseCase.AddService(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateService(ApiServiceSaveCommand service)
        {
            var command = ServiceUpdateCommandConverter.Convert(BusinessId, service);
            var response = ServiceUpdateUseCase.UpdateService(command);
            return CreatePostWebResponse(response);
        }
    }
}
