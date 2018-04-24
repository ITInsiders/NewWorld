using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NW.PL
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Quest",
                url: "Quest/{action}/{module}/{modal}",
                defaults: new { controller = "Quest", action = "Home", module = UrlParameter.Optional, modal = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Account",
                url: "Account/{action}/{id}",
                defaults: new { controller = "Account", action = "Account", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Map",
                url: "Map/{action}/{id}",
                defaults: new { controller = "Map", action = "Map", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Home",
                url: "{action}/{id}",
                defaults: new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );

        }
    }
}
