using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Fryhard.DevConfZA2016.Common;
using log4net;
using Fryhard.DevConfZA2016.Web.State;
using Model;

namespace Fryhard.DevConfZA2016.Web.SignalR
{
    public class VoteHub : Hub
    {
        private static ILog _Log = LogManager.GetLogger(typeof(VoteHub));

        public void CastVote (int number)
        {
            string connectionId = Context.ConnectionId;

            try
            {
                string userAgent = Context.Headers["User-Agent"];
                string ip = Context.Request.GetRemoteIpAddress();

                //Read the value from the cookie that we paced when the visitor joined us
                string voterId = Context.RequestCookies["Voter"].Value;

                Vote vote = new Vote()
                {
                    ConnectionId = connectionId,
                    IpAddress = ip,
                    UserAgent = userAgent,
                    VoteDateTime = DateTime.Now,
                    VoterId = voterId,
                    VoteValue = number
                };

                //Publish the message to the Message Bus
                BusHost.Publish<Vote>(vote, BusTopic.NewVote);
            }
            catch (Exception ex)
            {
                _Log.Error("Failed to make vote " + number + " for connection " + connectionId + ". " + ex.Message, ex);
            }
        }
    }
}