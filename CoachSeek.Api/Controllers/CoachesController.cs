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
    public class CoachesController : BaseController
    {
        public ICoachGetUseCase CoachGetUseCase { get; set; }
        public ICoachAddUseCase CoachAddUseCase { get; set; }
        public ICoachUpdateUseCase CoachUpdateUseCase { get; set; }

        public CoachesController()
        { }

        public CoachesController(ICoachGetUseCase coachGetUseCase,
                                 ICoachAddUseCase coachAddUseCase,
                                 ICoachUpdateUseCase coachUpdateUseCase)
        {
            CoachGetUseCase = coachGetUseCase;
            CoachAddUseCase = coachAddUseCase;
            CoachUpdateUseCase = coachUpdateUseCase;
        }

        //// GET: api/Coaches
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/Coaches/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            CoachGetUseCase.BusinessId = BusinessId;
            var response = CoachGetUseCase.GetCoach(id);
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
            var command = CoachAddCommandConverter.Convert(BusinessId, coach);
            var response = CoachAddUseCase.AddCoach(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateCoach(ApiCoachSaveCommand coach)
        {
            var command = CoachUpdateCommandConverter.Convert(BusinessId, coach);
            var response = CoachUpdateUseCase.UpdateCoach(command);
            return CreatePostWebResponse(response);
        }
    }
}
