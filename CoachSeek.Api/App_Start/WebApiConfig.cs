using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;
using CoachSeek.Application.Configuration;
using CoachSeek.DataAccess.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CoachSeek.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            WebApiAutoMapperConfigurator.Configure();
            ApplicationAutoMapperConfigurator.Configure();
            DbAutoMapperConfigurator.Configure();

            config.Services.Replace(typeof(IHttpControllerActivator), new ServiceActivator(config));

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
