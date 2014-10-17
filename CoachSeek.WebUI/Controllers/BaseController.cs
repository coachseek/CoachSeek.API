using CoachSeek.Application.Contracts.Models.Responses;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.WebUI.Controllers
{
    public abstract class BaseController : ApiController
    {
        protected HttpResponseMessage CreateWebResponse(Response response)
        {
            if (response is NotFoundResponse)
                return Request.CreateResponse(HttpStatusCode.NotFound);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK, response.Business);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors[0]);
        }
    }
}