using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Common;

namespace CoachSeek.Api.Controllers
{
    public abstract class BaseController : ApiController
    {
        private Guid? _businessId;

        public Guid BusinessId
        {
            // Make BusinessId public and setable for unit testing.
            set { _businessId = value; }
            get
            {
                if (_businessId.HasValue)
                    return _businessId.Value;
                return ((CoachseekIdentity)RequestContext.Principal.Identity).BusinessId;
            }
        }

        protected HttpResponseMessage CreateNotFoundWebResponse()
        {
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }


        protected HttpResponseMessage CreateGetWebResponse<TData>(TData data) where TData : class
        {
            if (data == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        protected HttpResponseMessage CreatePostWebResponse<TData>(Response<TData> response) where TData : class
        {
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK, response.Data);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors);
        }


        protected HttpResponseMessage CreateWebErrorResponse<TData>(Response<TData> response) where TData : class
        {
            if (response is NotFoundResponse<TData>)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors);
        }

        protected HttpResponseMessage CreateWebSuccessResponse<TData>(Response<TData> response) where TData : class
        {
            return Request.CreateResponse(HttpStatusCode.OK, response.Data);
        }
    }
}