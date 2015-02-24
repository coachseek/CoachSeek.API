using System;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;

namespace CoachSeek.Api.Controllers
{
    public class CoachesController : BaseController
    {
        public ICoachesGetAllUseCase CoachesGetAllUseCase { get; set; }
        public ICoachGetByIdUseCase CoachGetByIdUseCase { get; set; }
        public ICoachAddUseCase CoachAddUseCase { get; set; }
        public ICoachUpdateUseCase CoachUpdateUseCase { get; set; }

        public CoachesController()
        { }

        public CoachesController(ICoachesGetAllUseCase coachesGetAllUseCase,
                                 ICoachGetByIdUseCase coachGetByIdUseCase,
                                 ICoachAddUseCase coachAddUseCase,
                                 ICoachUpdateUseCase coachUpdateUseCase)
        {
            CoachesGetAllUseCase = coachesGetAllUseCase;
            CoachGetByIdUseCase = coachGetByIdUseCase;
            CoachAddUseCase = coachAddUseCase;
            CoachUpdateUseCase = coachUpdateUseCase;
        }


        // GET: api/Coaches
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get()
        {
            CoachesGetAllUseCase.BusinessId = BusinessId;
            var response = CoachesGetAllUseCase.GetCoaches();
            return CreateGetWebResponse(response);
        }

        // GET: api/Coaches/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            CoachGetByIdUseCase.BusinessId = BusinessId;
            var response = CoachGetByIdUseCase.GetCoach(id);
            return CreateGetWebResponse(response);
        }

        // POST: api/Coaches
        [BasicAuthentication]
        [Authorize]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiCoachSaveCommand coach)
        {
            if (coach.IsNew())
                return AddCoach(coach);

            return UpdateCoach(coach);
        }


        private HttpResponseMessage AddCoach(ApiCoachSaveCommand coach)
        {
            var command = CoachAddCommandConverter.Convert(coach);
            CoachAddUseCase.BusinessId = BusinessId;
            var response = CoachAddUseCase.AddCoach(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateCoach(ApiCoachSaveCommand coach)
        {
            var command = CoachUpdateCommandConverter.Convert(coach);
            CoachUpdateUseCase.BusinessId = BusinessId;
            var response = CoachUpdateUseCase.UpdateCoach(command);
            return CreatePostWebResponse(response);
        }
    }
}
