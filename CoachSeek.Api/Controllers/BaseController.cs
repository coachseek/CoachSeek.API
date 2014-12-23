using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.Application.Contracts.Models.Responses;

namespace CoachSeek.Api.Controllers
{
    public abstract class BaseController : ApiController
    {
        protected HttpResponseMessage CreateWebResponse<TData>(Response<TData> response) where TData : class
        {
            if (response is NotFoundResponse<TData>)
                return Request.CreateResponse(HttpStatusCode.NotFound);

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