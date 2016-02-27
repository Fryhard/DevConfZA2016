using Fryhard.DevConfZA2016.Common;
using Fryhard.DevConfZA2016.Web.SignalR;
using Fryhard.DevConfZA2016.Web.State;
using Microsoft.AspNet.SignalR;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Fryhard.DevConfZA2016.Web.App_Start
{
    public class RabbitStartup
    {
        public static void Subscribe()
        {
            //Subscribe to the average result message queue. When there is a new message update the WebHost's state
            IDisposable avgVoteSubscription = BusHost.SubscribeAsync <AverageResult>(BusSubscription.NewAverageResult, BusTopic.NewAverageResult, 
                                                    (a) => UpdateAverage (a));

            SubscriptionHandler.Instance.AddSubscription("avgVote", avgVoteSubscription);

            //Subscribe to the average result message queue. When there is a new message update the WebHost's state
            IDisposable voteSubscription = BusHost.SubscribeAsync<VoteProcessed>(BusSubscription.VoteProcessed, BusTopic.VoteProcessed,
                                                    (v) => NotifyOfVoteProcessedUpdateAverage(v));
            SubscriptionHandler.Instance.AddSubscription("vote", voteSubscription);
        }

        public static Task UpdateAverage (AverageResult a)
        {
            return Task.Factory.StartNew(() =>
            {
                VotingState.CurrentAverage = a.Average;
                VotingState.LastUpdated = a.DateStamp;

                //Connect to the signalR hub
                var context = GlobalHost.ConnectionManager.GetHubContext<VoteHub>();

                //Update the client of their vote
                context.Clients.All.UpdateAverage(a.Average);
            });
        }

        public static Task NotifyOfVoteProcessedUpdateAverage(VoteProcessed v)
        {
            return Task.Factory.StartNew(() =>
            {
                //Connect to the signalR hub
                var context = GlobalHost.ConnectionManager.GetHubContext<VoteHub>();

                //Update the client of their vote
                context.Clients.Client(v.ConnectionId).DisplayVoteResult(v.OriginalVoteValue, true, v.CurrentAverage);
            });
        }


        public static void Dispose ()
        {
            SubscriptionHandler.Instance.Unsubscribe("avgVote");
            SubscriptionHandler.Instance.Unsubscribe("vote");
        }
    }
}