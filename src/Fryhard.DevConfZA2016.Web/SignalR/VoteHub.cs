using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Fryhard.DevConfZA2016.Web.SignalR
{
    public class VoteHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void CastVote (int number)
        {
            string userAgent = Context.Headers["User-Agent"];
            string ip = Context.Request.GetRemoteIpAddress();
            string connectionId = Context.ConnectionId;
            int vote = number;


            Clients.Client(connectionId).DisplayResult(vote, true, vote);
        }
    }
}