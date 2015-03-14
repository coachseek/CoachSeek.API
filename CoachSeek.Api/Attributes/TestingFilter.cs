using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CoachSeek.Api.Controllers;
using Coachseek.DataAccess.Authentication.TableStorage;
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
                controller.BusinessRepository = new DbTestBusinessRepository(); // new InMemoryBusinessRepository();
                controller.UserRepository = new AzureTestTableUserRepository();
#else
                controller.BusinessRepository = new DbTestBusinessRepository();
                controller.UserRepository = new AzureTestTableUserRepository();
#endif
            }
            else
            {
                controller.BusinessRepository = new DbBusinessRepository();
                controller.UserRepository = new AzureTableUserRepository();
            }

            base.OnActionExecuting(actionContext);
        }
    }
}