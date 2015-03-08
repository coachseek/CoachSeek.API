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
            ServicesGetAllUseCase.BusinessId = BusinessId;
            ServicesGetAllUseCase.BusinessRepository = BusinessRepository;

            var response = ServicesGetAllUseCase.GetServices();
            return CreateGetWebResponse(response);
        }

        // GET: api/Services/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            ServiceGetByIdUseCase.BusinessId = BusinessId;
            ServiceGetByIdUseCase.BusinessRepository = BusinessRepository;

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
            ServiceDeleteUseCase.BusinessId = BusinessId;
            ServiceDeleteUseCase.BusinessRepository = BusinessRepository;

            var response = ServiceDeleteUseCase.DeleteService(id);
            return CreateDeleteWebResponse(response);
        }


        private HttpResponseMessage AddService(ApiServiceSaveCommand service)
        {
            var command = ServiceAddCommandConverter.Convert(service);
            ServiceAddUseCase.BusinessId = BusinessId;
            ServiceAddUseCase.BusinessRepository = BusinessRepository;

            var response = ServiceAddUseCase.AddService(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateService(ApiServiceSaveCommand service)
        {
            var command = ServiceUpdateCommandConverter.Convert(service);
            ServiceUpdateUseCase.BusinessId = BusinessId;
            ServiceUpdateUseCase.BusinessRepository = BusinessRepository;

            var response = ServiceUpdateUseCase.UpdateService(command);
            return CreatePostWebResponse(response);
        }
    }
}
