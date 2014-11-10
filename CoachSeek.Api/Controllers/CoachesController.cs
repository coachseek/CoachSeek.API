using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;

namespace CoachSeek.Api.Controllers
{
    public class CoachesController : BaseController
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
            var response = CoachAddUseCase.AddCoach(command);
            return CreateWebResponse(response);
        }

        private HttpResponseMessage UpdateCoach(ApiCoachSaveCommand coach)
        {
            var command = CoachUpdateCommandConverter.Convert(coach);
            var response = CoachUpdateUseCase.UpdateCoach(command);
            return CreateWebResponse(response);
        }
    }
}
