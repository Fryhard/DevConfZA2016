﻿using Fryhard.DevConfZA2016.Common;
using Fryhard.DevConfZA2016.Web.App_Start;
using Fryhard.DevConfZA2016.Web.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Fryhard.DevConfZA2016.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RabbitStartup.Subscribe();
            VotingState.Init();
        }

        protected void Application_End()
        {
            BusHost.Dispose();
            RabbitStartup.Dispose();
        }
    }
}