using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Scheduling;
using CoachSeek.Application.Contracts.UseCases;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.Data.Model;

namespace CoachSeek.Api.Controllers
{
    public class SessionsController : BaseController
    {
        public ISessionSearchUseCase SessionSearchUseCase { get; set; }
        public ISessionAddUseCase SessionAddUseCase { get; set; }
        public ISessionUpdateUseCase SessionUpdateUseCase { get; set; }


        public SessionsController()
        { }

        public SessionsController(ISessionSearchUseCase sessionSearchUseCase,
                                  ISessionAddUseCase sessionAddUseCase, 
                                  ISessionUpdateUseCase sessionUpdateUseCase)
        {
            SessionSearchUseCase = sessionSearchUseCase;
            SessionAddUseCase = sessionAddUseCase;
            SessionUpdateUseCase = sessionUpdateUseCase;
        }


        // GET: api/Sessions
        public HttpResponseMessage Get(string startDate, string endDate)
        {
            return SearchForSessions(startDate, endDate);
        }

        // GET: api/Sessions/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Services
        [BasicAuthentication]
        [Authorize]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiSessionSaveCommand session)
        {
            if (session.IsNew())
                return AddSession(session);

            return UpdateSession(session);
        }

        // PUT: api/Sessions/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Sessions/5
        public void Delete(int id)
        {
        }


        private HttpResponseMessage SearchForSessions(string startDate, string endDate)
        {
            SessionSearchUseCase.BusinessId = BusinessId;
            var response = SessionSearchUseCase.SearchForSessions(startDate, endDate);
            return CreateGetWebResponse(response);
        }

        private HttpResponseMessage AddSession(ApiSessionSaveCommand session)
        {
            var command = SessionAddCommandConverter.Convert(BusinessId, session);
            var response = SessionAddUseCase.AddSession(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateSession(ApiSessionSaveCommand session)
        {
            var command = SessionUpdateCommandConverter.Convert(BusinessId, session);
            var response = SessionUpdateUseCase.UpdateSession(command);
            return CreatePostWebResponse(response);
        }
    }
}
