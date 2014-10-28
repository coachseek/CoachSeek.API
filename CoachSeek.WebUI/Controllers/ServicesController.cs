﻿using System.Net.Http;
using System.Web.Http;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.WebUI.Conversion;
using CoachSeek.WebUI.Filters;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI.Controllers
{
    public class ServicesController : BaseController
    {
        public IServiceAddUseCase ServiceAddUseCase { get; set; }

        public ServicesController()
        { }

        public ServicesController(IServiceAddUseCase serviceAddUseCase)
        {
            ServiceAddUseCase = serviceAddUseCase;
        }


        // POST: api/Services
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiServiceSaveCommand service)
        {
            //if (service.IsNew())
                return AddService(service);

            //return UpdateCoach(service);
        }


        private HttpResponseMessage AddService(ApiServiceSaveCommand service)
        {
            var command = ServiceAddCommandConverter.Convert(service);
            var response = ServiceAddUseCase.AddService(command);
            return CreateWebResponse(response);
        }

        //private HttpResponseMessage UpdateService(ApiServiceSaveCommand service)
        //{
        //    var command = ServiceUpdateCommandConverter.Convert(service);
        //    var response = CoachUpdateUseCase.UpdateCoach(command);
        //    return CreateWebResponse(response);
        //}
    }
}
