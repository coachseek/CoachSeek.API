using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using CoachSeek.Common;
using CoachSeek.Domain.Entities.Authentication;

namespace CoachSeek.Api.Attributes
{
    public class BusinessAuthorizeAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            var identity = actionContext.RequestContext.Principal.Identity as CoachseekIdentity;
            if (identity == null)
                return;
            if(!IsAuthorised(identity))
                actionContext.Response = CreateForbiddenResponse(actionContext);
        }


        private bool IsAuthorised(CoachseekIdentity identity)
        {
            return identity.Business.AuthorisedUntil >= DateTime.UtcNow;
        }

        private HttpResponseMessage CreateForbiddenResponse(HttpActionContext actionContext)
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Forbidden,
                RequestMessage = actionContext.Request
            };
        }
    }
}