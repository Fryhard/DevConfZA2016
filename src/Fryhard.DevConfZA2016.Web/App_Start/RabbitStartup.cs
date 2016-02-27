using Fryhard.DevConfZA2016.Common;
using Fryhard.DevConfZA2016.Model;
using Fryhard.DevConfZA2016.Web.State;
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
                                                    (a) => 
                                                    { 
                                                        return Task.Factory.StartNew(() =>
                                                            {
                                                                VotingState.CurrentAverage = a.Average;
                                                                VotingState.LastUpdated = a.DateStamp;
                                                            });
                                                    });

            SubscriptionHandler.Instance.AddSubscription("avgVote", avgVoteSubscription);
        }

        public static void Dispose ()
        {
            SubscriptionHandler.Instance.Unsubscribe("avgVote");
        }
    }
}