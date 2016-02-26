using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fryhard.DevConfZA2016.Web.SignalR
{
    public static class Extensions
    {
        public static HttpContextBase GetHttpContext(this IRequest request)
        {
            object httpContextBaseValue;
            if (request.Environment.TryGetValue(typeof(HttpContextBase).FullName, out httpContextBaseValue))
            {
                return httpContextBaseValue as HttpContextBase;
            }

            return null;
        }


        public static string GetRemoteIpAddress(this IRequest request)
        {
            object ipAddress;
            if (request.Environment.TryGetValue("server.RemoteIpAddress", out ipAddress))
            {
                return ipAddress as string;
            }
            return null;
        }
    }
}