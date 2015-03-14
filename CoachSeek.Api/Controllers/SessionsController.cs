using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Scheduling;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Exceptions;
using System;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.Api.Controllers
{
    public class SessionsController : BaseController
    {
        public ISessionSearchUseCase SessionSearchUseCase { get; set; }
        public ISessionGetByIdUseCase SessionGetByIdUseCase { get; set; }
        public ISessionAddUseCase SessionAddUseCase { get; set; }
        public ISessionUpdateUseCase SessionUpdateUseCase { get; set; }
        public ISessionDeleteUseCase SessionDeleteUseCase { get; set; }


        public SessionsController()
        { }

        public SessionsController(ISessionSearchUseCase sessionSearchUseCase,
                                  ISessionGetByIdUseCase sessionGetByIdUseCase,
                                  ISessionAddUseCase sessionAddUseCase, 
                                  ISessionUpdateUseCase sessionUpdateUseCase,
                                  ISessionDeleteUseCase sessionDeleteUseCase)
        {
            SessionSearchUseCase = sessionSearchUseCase;
            SessionGetByIdUseCase = sessionGetByIdUseCase;
            SessionAddUseCase = sessionAddUseCase;
            SessionUpdateUseCase = sessionUpdateUseCase;
            SessionDeleteUseCase = sessionDeleteUseCase;
        }


        // GET: api/Sessions?startDate=2015-01-20&endDate=2015-01-26&coachId=AB73D488-2CAB-4B6D-A11A-9E98FF7A8FD8&locationId=DC39C46C-88DD-48E5-ADC4-2351634A5263
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null)
        {
            return SearchForSessions(startDate, endDate, coachId, locationId);
        }

        // GET: api/Sessions/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            SessionGetByIdUseCase.BusinessId = BusinessId;
            SessionGetByIdUseCase.BusinessRepository = BusinessRepository;

            var response = SessionGetByIdUseCase.GetSession(id);
            return CreateGetWebResponse(response);
        }

        // POST: api/Sessions
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

        // DELETE: api/Sessions/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Delete(Guid id)
        {
            SessionDeleteUseCase.BusinessId = BusinessId;
            SessionDeleteUseCase.BusinessRepository = BusinessRepository;

            var response = SessionDeleteUseCase.DeleteSession(id);
            return CreateDeleteWebResponse(response);
        }


        private HttpResponseMessage SearchForSessions(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null)
        {
            SessionSearchUseCase.BusinessId = BusinessId;
            SessionSearchUseCase.BusinessRepository = BusinessRepository;

            try
            {
                var response = SessionSearchUseCase.SearchForSessions(startDate, endDate, coachId, locationId);
                return CreateGetWebResponse(response);
            }
            catch (ValidationException ex)
            {
                return CreateGetErrorWebResponse(ex);
            }
        }

        private HttpResponseMessage AddSession(ApiSessionSaveCommand session)
        {
            var command = SessionAddCommandConverter.Convert(session);
            SessionAddUseCase.BusinessId = BusinessId;
            SessionAddUseCase.BusinessRepository = BusinessRepository;

            var response = SessionAddUseCase.AddSession(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateSession(ApiSessionSaveCommand session)
        {
            var command = SessionUpdateCommandConverter.Convert(session);
            SessionUpdateUseCase.BusinessId = BusinessId;
            SessionUpdateUseCase.BusinessRepository = BusinessRepository;

            var response = SessionUpdateUseCase.UpdateSession(command);
            return CreatePostWebResponse(response);
        }
    }
}
