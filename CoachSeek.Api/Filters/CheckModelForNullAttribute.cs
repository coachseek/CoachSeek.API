using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CoachSeek.Data.Model;

namespace CoachSeek.Api.Filters
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CheckModelForNullAttribute : ActionFilterAttribute
    {
        private readonly Func<Dictionary<string, object>, bool> _validate;

        public CheckModelForNullAttribute()
            : this(arguments => arguments.ContainsValue(null))
        { }

        public CheckModelForNullAttribute(Func<Dictionary<string, object>, bool> checkCondition)
        {
            _validate = checkCondition;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (_validate(actionContext.ActionArguments))
            {
                var errors = new [] { new ErrorData("Please post us some data!") };

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errors);
            }
        }
    }
}