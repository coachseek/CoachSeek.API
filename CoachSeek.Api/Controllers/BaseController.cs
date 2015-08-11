using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Api.Controllers
{
    public abstract class BaseController : ApiController
    {
        private BusinessDetails _business;
        private CurrencyDetails _currency;

        public IBusinessRepository BusinessRepository { set; protected get; }
        public IUserRepository UserRepository { set; protected get; }
        public ISupportedCurrencyRepository SupportedCurrencyRepository { set; protected get; }
        public IUnsubscribedEmailAddressRepository UnsubscribedEmailAddressRepository { set; protected get; }

        public BusinessDetails Business
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

        public CurrencyDetails Currency
        {
            set { _currency = value; }
            get
            {
                if (_currency != null)
                    return _currency;
                if (RequestContext.Principal.Identity is CoachseekIdentity)
                    return ((CoachseekIdentity)RequestContext.Principal.Identity).Currency;
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


        protected ApplicationContext Context
        {
            get
            {
                var userName = ControllerContext.RequestContext.Principal.Identity.Name;
                var businessContext = new BusinessContext(Business, Currency, userName, BusinessRepository, SupportedCurrencyRepository, UserRepository);
                var emailContext = new EmailContext(IsEmailingEnabled, ForceEmail, EmailSender, UnsubscribedEmailAddressRepository);

                return new ApplicationContext(businessContext, emailContext, IsTesting);
            }            
        }


        protected HttpResponseMessage CreateNotFoundWebResponse()
        {
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        protected HttpResponseMessage CreateGetErrorWebResponse(ValidationException errors)
        {
            return Request.CreateResponse(HttpStatusCode.BadRequest, errors.ToData());
        }

        protected HttpResponseMessage CreateGetWebResponse(object data)
        {
            if (data == null)
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

        protected HttpResponseMessage CreateWebSuccessResponse(Response response)
        {
            return Request.CreateResponse(HttpStatusCode.OK, response.Data);
        }
    }
}