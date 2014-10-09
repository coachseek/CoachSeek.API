using CoachSeek.WebUI.Contracts.UseCases;
using CoachSeek.WebUI.Conversion;
using CoachSeek.WebUI.Models.Api;
using CoachSeek.WebUI.UseCases;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.WebUI.Controllers
{
    public class CoachesController : ApiController
    {
        private ICoachAddUseCase CoachAddUseCase { get; set; }

        public CoachesController(ICoachAddUseCase coachAddUseCase)
        {
            CoachAddUseCase = coachAddUseCase;
        }

        // GET: api/Coaches
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Coaches/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Coaches
        public HttpResponseMessage Post([FromBody]ApiCoachSaveRequest coachSaveRequest)
        {
            if (coachSaveRequest.IsNew())
                return AddCoach(coachSaveRequest);

            return UpdateCoach(coachSaveRequest);
        }

        private HttpResponseMessage AddCoach(ApiCoachSaveRequest coachSaveRequest)
        {
            var coachAddRequest = CoachAddRequestConverter.Convert(coachSaveRequest);
            var response = CoachAddUseCase.AddCoach(coachAddRequest);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK, response.Business);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors[0]);
        }

        private HttpResponseMessage UpdateCoach(ApiCoachSaveRequest coachSaveRequest)
        {
            //var coachUpdateRequest = CoachUpdateRequestConverter.Convert(coachSaveRequest);
            //var updateUseCase = new CoachUpdateUseCase(BusinessRepository);
            //var response = updateUseCase.UpdateCoach(coachUpdateRequest);
            //if (response.IsSuccessful)
            //    return Request.CreateResponse(HttpStatusCode.OK, response.Business);
            //return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors[0]);

            return null;
        }

        // PUT: api/Coaches/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Coaches/5
        public void Delete(int id)
        {
        }
    }
}
