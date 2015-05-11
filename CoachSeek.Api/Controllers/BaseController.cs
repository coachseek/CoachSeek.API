﻿using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Common;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Api.Controllers
{
    public abstract class BaseController : ApiController
    {
        private Guid? _businessId;

        public IBusinessRepository BusinessRepository { set; protected get; }
        public IUserRepository UserRepository { set; protected get; }

        public Guid? BusinessId
        {
            // Make BusinessId public and setable for unit testing.
            set { _businessId = value; }
            get
            {
                if (_businessId.HasValue)
                    return _businessId;
                if (RequestContext.Principal.Identity is CoachseekIdentity)
                    return ((CoachseekIdentity)RequestContext.Principal.Identity).BusinessId;
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

        protected ApplicationContext Context
        {
            get
            {
                return new ApplicationContext
                {
                    BusinessId = BusinessId,
                    IsTesting = IsTesting,
                    ForceEmail = ForceEmail,
                    EmailSender = EmailSender,
                    BusinessRepository = BusinessRepository
                };
            }            
        }


        //protected ApplicationContext CreateContext()
        //{
        //    return new ApplicationContext
        //    {
        //        BusinessId = BusinessId,
        //        IsTesting = IsTesting,
        //        ForceEmail = ForceEmail,
        //        EmailSender = EmailSender,
        //        BusinessRepository = BusinessRepository
        //    };
        //}

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

        protected HttpResponseMessage CreatePostWebResponse(Response response)
        {
            if (response is NotFoundResponse)
                return Request.CreateResponse(HttpStatusCode.NotFound);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK, response.Data);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors);
        }

        protected HttpResponseMessage CreateDeleteWebResponse(Response response)
        {
            if (response is NotFoundResponse)
                return Request.CreateResponse(HttpStatusCode.NotFound);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors);
        }


        protected HttpResponseMessage CreateWebErrorResponse(Response response)
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