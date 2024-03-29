﻿using System.Threading.Tasks;
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
    public class CoachesController : BaseController
    {
        public ICoachesGetAllUseCase CoachesGetAllUseCase { get; set; }
        public ICoachGetByIdUseCase CoachGetByIdUseCase { get; set; }
        public ICoachAddUseCase CoachAddUseCase { get; set; }
        public ICoachUpdateUseCase CoachUpdateUseCase { get; set; }
        public ICoachDeleteUseCase CoachDeleteUseCase { get; set; }

        public CoachesController()
        { }

        public CoachesController(ICoachesGetAllUseCase coachesGetAllUseCase,
                                 ICoachGetByIdUseCase coachGetByIdUseCase,
                                 ICoachAddUseCase coachAddUseCase,
                                 ICoachUpdateUseCase coachUpdateUseCase,
                                 ICoachDeleteUseCase coachDeleteUseCase)
        {
            CoachesGetAllUseCase = coachesGetAllUseCase;
            CoachGetByIdUseCase = coachGetByIdUseCase;
            CoachAddUseCase = coachAddUseCase;
            CoachUpdateUseCase = coachUpdateUseCase;
            CoachDeleteUseCase = coachDeleteUseCase;
        }


        // GET: Coaches
        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        public async Task<HttpResponseMessage> GetAsync()
        {
            CoachesGetAllUseCase.Initialise(Context);
            var response = await CoachesGetAllUseCase.GetCoachesAsync();
            return CreateGetWebResponse(response);
        }

        // GET: Coaches/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        public async Task<HttpResponseMessage> GetAsync(Guid id)
        {
            CoachGetByIdUseCase.Initialise(Context);
            var response = await CoachGetByIdUseCase.GetCoachAsync(id);
            return CreateGetWebResponse(response);
        }

        // POST: Coaches
        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiCoachSaveCommand coach)
        {
            if (coach.IsNew())
                return AddCoach(coach);

            return UpdateCoach(coach);
        }

        // DELETE: Coaches/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        public async Task<HttpResponseMessage> DeleteAsync(Guid id)
        {
            CoachDeleteUseCase.Initialise(Context);
            var response = await CoachDeleteUseCase.DeleteCoachAsync(id);
            return CreateDeleteWebResponse(response);
        }


        private HttpResponseMessage AddCoach(ApiCoachSaveCommand coach)
        {
            var command = CoachAddCommandConverter.Convert(coach);
            CoachAddUseCase.Initialise(Context);
            var response = CoachAddUseCase.AddCoach(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateCoach(ApiCoachSaveCommand coach)
        {
            var command = CoachUpdateCommandConverter.Convert(coach);
            CoachUpdateUseCase.Initialise(Context);
            var response = CoachUpdateUseCase.UpdateCoach(command);
            return CreatePostWebResponse(response);
        }
    }
}
