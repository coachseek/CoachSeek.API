using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Conversion.Out;
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


        // GET: Sessions?startDate=2015-01-20&endDate=2015-01-26&coachId=AB73D488-2CAB-4B6D-A11A-9E98FF7A8FD8&locationId=DC39C46C-88DD-48E5-ADC4-2351634A5263
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null, Guid? serviceId = null, bool? useNewSearch = null)
        {
            return SearchForSessions(startDate, endDate, coachId, locationId, serviceId, useNewSearch);
        }

        // GET: OnlineBooking/Sessions?startDate=2015-01-20&endDate=2015-01-26&coachId=AB73D488-2CAB-4B6D-A11A-9E98FF7A8FD8&locationId=DC39C46C-88DD-48E5-ADC4-2351634A5263
        [Route("OnlineBooking/Sessions")]
        [BasicAuthenticationOrAnonymous]
        [Authorize]
        public HttpResponseMessage GetForOnlineBooking(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null, Guid? serviceId = null)
        {
            return SearchForOnlineBookableSessions(startDate, endDate, coachId, locationId, serviceId);
        }

        // GET: Sessions/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            SessionGetByIdUseCase.Initialise(Context);
            var response = SessionGetByIdUseCase.GetSession(id);
            var apiSessionResponse = ApiOutSessionConverter.Convert(response);
            return CreateGetWebResponse(apiSessionResponse);
        }

        // POST: Sessions
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

        // DELETE: Sessions/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Delete(Guid id)
        {
            SessionDeleteUseCase.Initialise(Context);
            var response = SessionDeleteUseCase.DeleteSession(id);
            return CreateDeleteWebResponse(response);
        }


        private HttpResponseMessage SearchForSessions(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null, Guid? serviceId = null, bool? useNewSearch = null)
        {
            SessionSearchUseCase.Initialise(Context);

            try
            {
                // TODO: Remove if on useNewSearch
                if (!useNewSearch.HasValue)
                {
                    // Old search results. TODO: Remove
                    var responseOld = SessionSearchUseCase.SearchForSessionsOld(startDate, endDate, coachId, locationId, serviceId);
                    return CreateGetWebResponse(responseOld);
                }

                var response = SessionSearchUseCase.SearchForSessions(startDate, endDate, coachId, locationId, serviceId);
                var apiSearchResponse = ApiOutSessionSearchResultConverter.Convert(response);
                return CreateGetWebResponse(apiSearchResponse);
            }
            catch (ValidationException ex)
            {
                return CreateGetErrorWebResponse(ex);
            }
        }

        private HttpResponseMessage SearchForOnlineBookableSessions(string startDate, string endDate, Guid? coachId = null, Guid? locationId = null, Guid? serviceId = null)
        {
            SessionSearchUseCase.Initialise(Context);

            try
            {
                var response = SessionSearchUseCase.SearchForOnlineBookableSessions(startDate, endDate, coachId, locationId, serviceId);
                var apiSearchResponse = ApiOutOnlineBookingSessionSearchResultConverter.Convert(response);
                return CreateGetWebResponse(apiSearchResponse);
            }
            catch (ValidationException ex)
            {
                return CreateGetErrorWebResponse(ex);
            }
        }

        private HttpResponseMessage AddSession(ApiSessionSaveCommand session)
        {
            var command = SessionAddCommandConverter.Convert(session);
            SessionAddUseCase.Initialise(Context);
            var response = SessionAddUseCase.AddSession(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateSession(ApiSessionSaveCommand session)
        {
            var command = SessionUpdateCommandConverter.Convert(session);
            SessionUpdateUseCase.Initialise(Context);
            var response = SessionUpdateUseCase.UpdateSession(command);
            return CreatePostWebResponse(response);
        }
    }
}
