using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Fryhard.DevConfZA2016.Web.App_Start.SignalRStartup))]
namespace Fryhard.DevConfZA2016.Web.App_Start
{
    public class SignalRStartup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}