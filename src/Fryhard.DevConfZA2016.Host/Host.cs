using Fryhard.DevConfZA2016.Common;
using Fryhard.DevConfZA2016.Host.IEC;
using Fryhard.DevConfZA2016.Model;
using log4net;
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

                IDisposable voteSubscription = BusHost.SubscribeAsync<Vote>(BusSubscription.NewVote, BusTopic.NewVote, msg => _VoteProcessor.ProcessVote(msg));
                _Log.Debug("External processing service subscribed to new leads");

                SubscriptionHandler.Instance.AddSubscription("vote", voteSubscription);
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
