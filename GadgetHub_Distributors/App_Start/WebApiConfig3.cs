using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GadgetHub_Distributors
{
    public static class WebApiConfig3
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
