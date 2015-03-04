using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CoachSeek.Api.Controllers;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using Coachseek.DataAccess.Main.SqlServer.Repositories;

namespace CoachSeek.Api.Attributes
{
    public class TestingFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var isTesting = actionContext.Request.Headers.Contains("Testing");

            var controller = (BaseController)actionContext.ControllerContext.Controller;

            if (isTesting)
            {
#if DEBUG
                controller.BusinessRepository = new DbTestBusinessRepository();
                //controller.BusinessRepository = new InMemoryBusinessRepository();
#else
                controller.BusinessRepository = new DbTestBusinessRepository();
#endif
            }
            else
                controller.BusinessRepository = new DbBusinessRepository();

            base.OnActionExecuting(actionContext);
        }
    }
}