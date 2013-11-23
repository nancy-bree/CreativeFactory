using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CreativeFactory.DAL;
using CreativeFactory.Web.Infrastructure;
using WebMatrix.WebData;
using CreativeFactory.Web.Helpers;
using System.Threading;
using System.Globalization;

namespace CreativeFactory.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            var context = new CreativeFactoryContext();
            if (!WebSecurity.Initialized)
                WebSecurity.InitializeDatabaseConnection("CreativeFactoryContext",
                    "Users", "Id", "UserName", autoCreateTables: true);

            ControllerBuilder.Current.SetControllerFactory(new CreativeFactoryControllerFactory());
            BootStrapper.ConfigureDependencies();
        }
    }
}