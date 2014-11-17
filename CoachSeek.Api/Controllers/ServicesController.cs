﻿using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;

namespace CoachSeek.Api.Controllers
{
    public class ServicesController : BaseController
    {
        public IServiceAddUseCase ServiceAddUseCase { get; set; }
        public IServiceUpdateUseCase ServiceUpdateUseCase { get; set; }

        public ServicesController()
        { }

        public ServicesController(IServiceAddUseCase serviceAddUseCase, IServiceUpdateUseCase serviceUpdateUseCase)
        {
            ServiceAddUseCase = serviceAddUseCase;
            ServiceUpdateUseCase = serviceUpdateUseCase;
        }


        // POST: api/Services
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
            var command = ServiceAddCommandConverter.Convert(service);
            var response = ServiceAddUseCase.AddService(command);
            return CreateWebResponse(response);
        }

        private HttpResponseMessage UpdateService(ApiServiceSaveCommand service)
        {
            var command = ServiceUpdateCommandConverter.Convert(service);
            var response = ServiceUpdateUseCase.UpdateService(command);
            return CreateWebResponse(response);
        }
    }
}