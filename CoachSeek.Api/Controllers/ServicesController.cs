using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;
using System;
using System.Net.Http;
using System.Web.Http;

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


        // GET: api/Services
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get()
        {
            ServicesGetAllUseCase.Initialise(BusinessRepository, BusinessId);
            var response = ServicesGetAllUseCase.GetServices();
            return CreateGetWebResponse(response);
        }

        // GET: api/Services/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            ServiceGetByIdUseCase.Initialise(BusinessRepository, BusinessId);
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

        // DELETE: api/Services/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Delete(Guid id)
        {
            ServiceDeleteUseCase.Initialise(BusinessRepository, BusinessId);
            var response = ServiceDeleteUseCase.DeleteService(id);
            return CreateDeleteWebResponse(response);
        }


        private HttpResponseMessage AddService(ApiServiceSaveCommand service)
        {
            var command = ServiceAddCommandConverter.Convert(service);
            ServiceAddUseCase.Initialise(BusinessRepository, BusinessId);
            var response = ServiceAddUseCase.AddService(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateService(ApiServiceSaveCommand service)
        {
            var command = ServiceUpdateCommandConverter.Convert(service);
            ServiceUpdateUseCase.Initialise(BusinessRepository, BusinessId);
            var response = ServiceUpdateUseCase.UpdateService(command);
            return CreatePostWebResponse(response);
        }
    }
}
