using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.WebUI.Conversion;
using CoachSeek.WebUI.Filters;
using CoachSeek.WebUI.Models.Api.Scheduling;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.WebUI.Controllers
{
    public class SessionsController : BaseController
    {
        public ISessionAddUseCase SessionAddUseCase { get; set; }


        public SessionsController()
        { }

        public SessionsController(ISessionAddUseCase sessionAddUseCase)
        {
            SessionAddUseCase = sessionAddUseCase;
        }


        // GET: api/Sessions
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Sessions/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Services
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiSessionSaveCommand session)
        {
            if (session.IsNew())
                return AddSession(session);

            return null;
        }

        // PUT: api/Sessions/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Sessions/5
        public void Delete(int id)
        {
        }



        private HttpResponseMessage AddSession(ApiSessionSaveCommand session)
        {
            var command = SessionAddCommandConverter.Convert(session);
            var response = SessionAddUseCase.AddSession(command);
            return CreateWebResponse(response);
        }

        //private HttpResponseMessage UpdateSession(ApiSessionSaveCommand session)
        //{
        //    var command = CoachUpdateCommandConverter.Convert(coach);
        //    var response = CoachUpdateUseCase.UpdateCoach(command);
        //    return CreateWebResponse(response);
        //}
    }
}
