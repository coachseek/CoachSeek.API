using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CoachSeek.Data.Model;

namespace CoachSeek.Api.Filters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errors = RepackageErrors(actionContext);

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errors);
            }
        }

        private static List<ErrorData> RepackageErrors(HttpActionContext actionContext)
        {
            var errors = new List<ErrorData>();

            foreach (var key in actionContext.ModelState.Keys)
            {
                var value = actionContext.ModelState[key];
                foreach (var error in value.Errors)
                {
                    var errorData = new ErrorData(key, error.ErrorMessage);
                    errors.Add(errorData);
                }
            }
            return errors;
        }
    }
}