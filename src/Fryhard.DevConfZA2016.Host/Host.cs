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

                Subscribe();

                IDisposable controlSubscription = BusHost.SubscribeAsync<Control>(BusSubscription.Control, BusTopic.Control, msg => ProcessControl(msg));
                SubscriptionHandler.Instance.AddSubscription("Control", controlSubscription);
            }
            catch (Exception ex)
            {
                _Log.Error("Failed to start Host. " + ex.Message, ex);
            }
        }

        public void Subscribe ()
        {
            IDisposable goodVoteSubscription = BusHost.SubscribeAsync<Vote>(BusSubscription.GoodVote, BusTopic.NewVote, msg => _VoteProcessor.ProcessGoodVote(msg));
            SubscriptionHandler.Instance.AddSubscription("GoodVote", goodVoteSubscription);

            IDisposable badVoteSubscription = BusHost.SubscribeAsync<Vote>(BusSubscription.BadVote, BusTopic.NewVote, msg => _VoteProcessor.ProcessBadVote(msg));
            SubscriptionHandler.Instance.AddSubscription("BadVote", badVoteSubscription);

            IDisposable saveVoteSubscription = BusHost.SubscribeAsync<Vote>(BusSubscription.SaveVote, BusTopic.NewVote, msg => VoteSaver.ProcessVote(msg));
            SubscriptionHandler.Instance.AddSubscription("SaveVote", saveVoteSubscription);
        }

        public Task ProcessControl (Control control)
        {
            return Task.Factory.StartNew(() =>
            {
                if (control.Stop)
                {
                    SubscriptionHandler.Instance.Unsubscribe("GoodVote");
                    SubscriptionHandler.Instance.Unsubscribe("BadVote");
                    SubscriptionHandler.Instance.Unsubscribe("SaveVote");
                }
                    
                if (control.Start)
                {
                    Subscribe();
                }

                if (control.Reset && _VoteProcessor != null)
                {
                    _VoteProcessor.Reset();
                }
            });
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
