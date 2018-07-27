using ChipMongWebApp.Utils.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ChipMongWebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //config.Filters.Add(new ErrorLoggerAttribute());
        }
    }
}
