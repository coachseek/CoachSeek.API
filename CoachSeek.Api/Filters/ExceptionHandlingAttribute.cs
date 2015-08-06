using System.Diagnostics;
using System.Web.Http.Filters;

namespace CoachSeek.Api.Filters
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            Trace.WriteLine(context.Exception, "Error");

            base.OnException(context);
        }
    }
}