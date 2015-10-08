using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;
using CoachSeek.Common;
using CoachSeek.Domain.Entities.Authentication;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Api.Attributes
{
    public class BusinessAuthorizeAttribute : AuthorizeAttribute
    {
        private new List<Role> Roles { get; set; }

        public BusinessAuthorizeAttribute(params Role[] roles)
        {
            Roles = new List<Role>(roles);
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            Authorise(actionContext);
        }


        private void Authorise(HttpActionContext actionContext)
        {
            var principal = actionContext.RequestContext.Principal;
            var identity = principal.Identity as CoachseekIdentity;
            if (identity == null)
                return;
            if (IsExpired(identity.Business.AuthorisedUntil))
                actionContext.Response = CreateExpiredForbiddenResponse(actionContext);
            if (!IsInRole(principal))
                actionContext.Response = CreateUnauthorisedForbiddenResponse(actionContext);
        }

        private bool IsExpired(DateTime authorisedUntil)
        {
            return authorisedUntil < DateTime.UtcNow;
        }

        private bool IsInRole(IPrincipal principal)
        {
            return Roles.Any(role => principal.IsInRole(role.ToString()));
        }

        private HttpResponseMessage CreateForbiddenResponse(HttpActionContext actionContext, Error error)
        {
            return actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, error);
        }

        private HttpResponseMessage CreateExpiredForbiddenResponse(HttpActionContext actionContext)
        {
            var error = new Error("license-expired", "Your Coachseek license has expired.");
            return CreateForbiddenResponse(actionContext, error);
        }

        private HttpResponseMessage CreateUnauthorisedForbiddenResponse(HttpActionContext actionContext)
        {
            var error = new Error("unauthorised-action", "You're not authorised for this action.");
            return CreateForbiddenResponse(actionContext, error);
        }
    }
}