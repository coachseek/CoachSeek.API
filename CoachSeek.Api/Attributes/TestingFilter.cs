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
            var repositories = DataAccessFactory.CreateDataAccess(isTesting);

            var controller = (BaseController)actionContext.ControllerContext.Controller;
            controller.BusinessRepository = repositories.Item1;
            controller.UserRepository = repositories.Item2;

            base.OnActionExecuting(actionContext);
        }
    }
}