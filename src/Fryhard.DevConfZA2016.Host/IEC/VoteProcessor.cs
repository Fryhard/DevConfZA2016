using Fryhard.DevConfZA2016.Common;
using Fryhard.DevConfZA2016.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fryhard.DevConfZA2016.Host.IEC
{
    public class VoteProcessor
    {
        private static readonly ILog _Log = LogManager.GetLogger(typeof(Host));

        private List<Vote> _Votes { get; set; }

        public VoteProcessor ()
        {
            _Votes = new List<Vote>();
        }

        public AverageResult GetAverageResult ()
        {
            double avgD = _Votes.Average(v => v.VoteValue);
            _Log.Debug("Avg = " + avgD);

            int avg = (int) Math.Round(avgD);
            return new AverageResult ()
            {
                Average = avg,
                DateStamp = DateTime.Now
            };
        }

        public Task ProcessVote(Vote vote)
        {
            return Task.Factory.StartNew(() =>
            {
                _Votes.Add(vote);

                BusHost.Publish(GetAverageResult(), BusTopic.NewAverageResult);

            });
        }
    }
}
