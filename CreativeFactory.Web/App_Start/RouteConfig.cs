﻿using CreativeFactory.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CreativeFactory.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "Default",
            url: "{culture}/{controller}/{action}/{id}",
            defaults: new { culture = String.Empty, controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}