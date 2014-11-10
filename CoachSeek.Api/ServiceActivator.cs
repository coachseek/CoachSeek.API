using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using StructureMap;

namespace CoachSeek.Api
{
    public class ServiceActivator : IHttpControllerActivator
    {
        public ServiceActivator(HttpConfiguration configuration) { }

        public IHttpController Create(HttpRequestMessage request
            , HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var container = new Container(new TypeRegistry());
            return container.GetInstance(controllerType) as IHttpController;
        }
    }
}