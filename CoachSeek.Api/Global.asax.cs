using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using CoachSeek.Api.Filters;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Common.Services.Authentication;
using Coachseek.DataAccess.TableStorage.Logging;
using Coachseek.Logging.Contracts;
using StructureMap;

namespace CoachSeek.Api
{
    public class MvcApplication : HttpApplication
    { 
        private bool IsRequestLoggingEnabled
        {
            get { return AppSettings.IsRequestLoggingEnabled.Parse(false); }
        }


        // Static IoC container property needed for attributes.
        public static Container IocContainer
        {
            get { return new Container(new TypeRegistry()); }
        }

        public static void RegisterWebApiFilters(HttpFilterCollection filters)
        {
            filters.Add(new RequireHttpsAttribute());
            filters.Add(new ExceptionHandlingAttribute());
        }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            RegisterWebApiFilters(GlobalConfiguration.Configuration.Filters);
        }

        protected void Application_EndRequest()
        {
            if (IsRequestLoggingEnabled)
                LogHttpRequest();
        }

        private void LogHttpRequest()
        {
            var message = new RequestLogMessage(HttpContext.Current);
            var repository = CreateRequestLogRepository(HttpContext.Current.Request);
            repository.Log(message);
        }


        private static AzureTableHttpRequestLogRepository CreateRequestLogRepository(HttpRequest request)
        {
            var isTesting = request.Headers.AllKeys.Contains("Testing");
            var repository = isTesting
                ? new AzureTestTableHttpRequestLogRepository()
                : new AzureTableHttpRequestLogRepository();
            return repository;
        }
    }
}
