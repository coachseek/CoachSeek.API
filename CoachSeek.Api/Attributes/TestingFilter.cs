using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CoachSeek.Api.Controllers;

namespace CoachSeek.Api.Attributes
{
    public class TestingFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            SetRepositoriesOnBaseController(actionContext);

            base.OnActionExecuting(actionContext);
        }

        private void SetRepositoriesOnBaseController(HttpActionContext actionContext)
        {
            var isTesting = actionContext.Request.Headers.Contains("Testing");
            var repositories = DataAccessFactory.CreateDataAccess(isTesting);

            var controller = (BaseController)actionContext.ControllerContext.Controller;
            controller.BusinessRepository = repositories.BusinessRepository;
            controller.UserRepository = repositories.UserRepository;
            controller.SupportedCurrencyRepository = repositories.SupportedCurrencyRepository;
            controller.UnsubscribedEmailAddressRepository = repositories.UnsubscribedEmailAddressRepository;
        }
    }
}