using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Fryhard.DevConfZA2016.Common;
using log4net;
using Fryhard.DevConfZA2016.Web.State;
using Fryhard.DevConfZA2016.Model;

namespace Fryhard.DevConfZA2016.Web.SignalR
{
    public class VoteHub : Hub
    {
        private static ILog _Log = LogManager.GetLogger(typeof(VoteHub));

        public void CastVote (int number)
        {
            string connectionId = Context.ConnectionId;

            bool success = true;
            try
            {
                string userAgent = Context.Headers["User-Agent"];
                string ip = Context.Request.GetRemoteIpAddress();

                Vote vote = new Vote()
                {
                    ConnectionId = connectionId,
                    IpAddress = ip,
                    UserAgent = userAgent,
                    VoteDateTime = DateTime.Now,
                    VoteValue = number
                };

                //Publish the message to the Message Bus
                BusHost.Publish<Vote>(vote, BusTopic.NewVote);
            }
            catch (Exception ex)
            {
                success = false;
                _Log.Error("Failed to make vote " + number + " for connection " + connectionId + ". " + ex.Message, ex);
            }

            //Return a response to the client
            Clients.Client(connectionId).DisplayVoteResult(number, success, VotingState.CurrentAverage);
        }
    }
}