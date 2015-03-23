using System.Web.Http;
using StructureMap;

namespace CoachSeek.Api
{
    public class MvcApplication : System.Web.HttpApplication
    {
        // Static IoC container property needed for attributes.
        public static Container IocContainer
        {
            get { return new Container(new TypeRegistry()); }
        }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
