using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using CoachSeek.Api.Filters;
using StructureMap;

namespace CoachSeek.Api
{
    public class MvcApplication : HttpApplication
    {
        // Static IoC container property needed for attributes.
        public static Container IocContainer
        {
            get { return new Container(new TypeRegistry()); }
        }

        public static void RegisterWebApiFilters(HttpFilterCollection filters)
        {
            filters.Add(new RequireHttpsAttribute());
        }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            RegisterWebApiFilters(GlobalConfiguration.Configuration.Filters);
        }
    }
}
