using Fryhard.DevConfZA2016.Common;
using Fryhard.DevConfZA2016.Host.IEC;
using log4net;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fryhard.DevConfZA2016.Host
{
    public class Host
    {
        private static readonly ILog _Log = LogManager.GetLogger(typeof(Host));
        private static VoteProcessor _VoteProcessor;


        public Host()
        {
            _VoteProcessor = new VoteProcessor();
        }

        public virtual void Start()
        {
            try
            {
                BusHost.Register();
                _Log.Debug("Bus registered");

                IDisposable goodVoteSubscription = BusHost.SubscribeAsync<Vote>(BusSubscription.GoodVote, BusTopic.NewVote, msg => _VoteProcessor.ProcessGoodVote(msg));
                SubscriptionHandler.Instance.AddSubscription("GoodVote", goodVoteSubscription);

                IDisposable badVoteSubscription = BusHost.SubscribeAsync<Vote>(BusSubscription.BadVote, BusTopic.NewVote, msg => _VoteProcessor.ProcessBadVote(msg));
                SubscriptionHandler.Instance.AddSubscription("BadVote", badVoteSubscription);

                IDisposable saveVoteSubscription = BusHost.SubscribeAsync<Vote>(BusSubscription.SaveVote, BusTopic.NewVote, msg => VoteSaver.ProcessVote(msg));
                SubscriptionHandler.Instance.AddSubscription("saveVote", saveVoteSubscription);
            }
            catch (Exception ex)
            {
                _Log.Error("Failed to start Host. " + ex.Message, ex);
            }
        }


        public virtual void Stop()
        {
            try
            {
                BusHost.Dispose();
                _Log.Debug("BusHost disposed");
            }
            catch (Exception ex)
            {
                _Log.Error("An error occurred while trying to Stop Host. " + ex.Message, ex);
            }
        }
    }
}
