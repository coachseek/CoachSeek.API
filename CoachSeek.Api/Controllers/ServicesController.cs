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
        public IServiceGetUseCase ServiceGetUseCase { get; set; }
        public IServiceAddUseCase ServiceAddUseCase { get; set; }
        public IServiceUpdateUseCase ServiceUpdateUseCase { get; set; }

        public ServicesController()
        { }

        public ServicesController(IServiceGetUseCase serviceGetUseCase,
                                  IServiceAddUseCase serviceAddUseCase, 
                                  IServiceUpdateUseCase serviceUpdateUseCase)
        {
            ServiceGetUseCase = serviceGetUseCase;
            ServiceAddUseCase = serviceAddUseCase;
            ServiceUpdateUseCase = serviceUpdateUseCase;
        }


        // GET: api/Services/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            ServiceGetUseCase.BusinessId = BusinessId;
            var response = ServiceGetUseCase.GetService(id);
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
