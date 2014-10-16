﻿using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.WebUI.Conversion;
using CoachSeek.WebUI.Models.Api;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.WebUI.Controllers
{
    public class CoachesController : ApiController
    {
        public ICoachAddUseCase CoachAddUseCase { get; set; }
        public ICoachUpdateUseCase CoachUpdateUseCase { get; set; }

        public CoachesController()
        { }

        public CoachesController(ICoachAddUseCase coachAddUseCase,
                                 ICoachUpdateUseCase coachUpdateUseCase)
        {
            CoachAddUseCase = coachAddUseCase;
            CoachUpdateUseCase = coachUpdateUseCase;
        }

        //// GET: api/Coaches
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Coaches/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Coaches
        public HttpResponseMessage Post([FromBody]ApiCoachSaveCommand coachSaveCommand)
        {
            if (coachSaveCommand.IsNew())
                return AddCoach(coachSaveCommand);

            return UpdateCoach(coachSaveCommand);
        }


        private HttpResponseMessage AddCoach(ApiCoachSaveCommand coachSaveCommand)
        {
            var command = CoachAddCommandConverter.Convert(coachSaveCommand);
            var response = CoachAddUseCase.AddCoach(command);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK, response.Business);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors[0]);
        }

        private HttpResponseMessage UpdateCoach(ApiCoachSaveCommand coachSaveCommand)
        {
            var command = CoachUpdateCommandConverter.Convert(coachSaveCommand);
            var response = CoachUpdateUseCase.UpdateCoach(command);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK, response.Business);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors[0]);
        }
    }
}
