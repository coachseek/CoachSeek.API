using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Entities.Authentication;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Api.Controllers
{
    public abstract class BaseController : ApiController
    {
        private Business _business;

        public IBusinessRepository BusinessRepository { set; protected get; }
        public IUserRepository UserRepository { set; protected get; }
        public ISupportedCurrencyRepository SupportedCurrencyRepository { set; protected get; }
        public IUnsubscribedEmailAddressRepository UnsubscribedEmailAddressRepository { set; protected get; }
        public ILogRepository LogRepository { set; protected get; }

        //public User CurrentUser
        //{
        //    // Make User public and setable for unit testing.
        //    set { _user = value; }
        //    get
        //    {
        //        if (_user != null)
        //            return _user;
        //        if (RequestContext.Principal.Identity is CoachseekIdentity)
        //            return ((CoachseekIdentity)RequestContext.Principal.Identity).User;
        //        return null;
        //    }
        //}

        public Business Business
        {
            // Make Business public and setable for unit testing.
            set { _business = value; }
            get
            {
                if (_business != null)
                    return _business;
                if (RequestContext.Principal.Identity is CoachseekIdentity)
                    return ((CoachseekIdentity)RequestContext.Principal.Identity).Business;
                return null;
            }
        }

        protected bool IsTesting
        {
            get { return Request.Headers.Contains("Testing"); }
        }

        protected bool ForceEmail
        {
            get { return Request.Headers.Contains("ForceEmail"); }
        }

        protected string EmailSender
        {
            get { return AppSettings.EmailSender; }
        }

        protected bool IsEmailingEnabled
        {
            get { return AppSettings.IsEmailingEnabled.Parse(true); }
        }

        protected bool IsUserTrackingEnabled
        {
            get { return AppSettings.IsUserTrackingEnabled.Parse(false); }
        }


        protected ApplicationContext Context
        {
            get 
            { 
                return new ApplicationContext(UserContext, 
                                              BusinessContext, 
                                              EmailContext, 
                                              LogRepository, 
                                              IsTesting); 
            }
        }


        protected UserContext UserContext
        {
            get { return new UserContext(UserRepository); }
        }

        private BusinessContext BusinessContext
        {
            get
            {
                if (Business == null)
                    return null;
                return new BusinessContext(Business, BusinessRepository, SupportedCurrencyRepository, UserRepository);
            }
        }

        protected EmailContext EmailContext
        {
            get { return new EmailContext(IsEmailingEnabled, ForceEmail, EmailSender, UnsubscribedEmailAddressRepository); }
        }

        protected HttpResponseMessage CreateNotFoundWebResponse()
        {
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        protected HttpResponseMessage CreateGetWebResponse(object data)
        {
            if (data.IsNotFound())
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        protected HttpResponseMessage CreatePostWebResponse(IResponse response)
        {
            if (response is NotFoundResponse)
                return Request.CreateResponse(HttpStatusCode.NotFound);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK, response.Data);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors);
        }

        protected HttpResponseMessage CreateDeleteWebResponse(IResponse response)
        {
            if (response is NotFoundResponse)
                return Request.CreateResponse(HttpStatusCode.NotFound);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors);
        }

        protected HttpResponseMessage CreateWebErrorResponse(IResponse response)
        {
            if (response is NotFoundResponse)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors);
        }

        protected HttpResponseMessage CreateWebErrorResponse(CoachseekException exception)
        {
            return Request.CreateResponse(HttpStatusCode.BadRequest, exception.Errors);
        }

        protected HttpResponseMessage CreateWebSuccessResponse(Response response)
        {
            return Request.CreateResponse(HttpStatusCode.OK, response.Data);
        }
    }
}