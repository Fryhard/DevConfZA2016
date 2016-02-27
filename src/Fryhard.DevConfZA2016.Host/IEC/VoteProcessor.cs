using Fryhard.DevConfZA2016.Common;
using log4net;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            double avgD = _Votes.Count == 0? 0 : _Votes.Average(v => v.VoteValue);
            _Log.Debug("Avg = " + avgD);

            int avg = (int) Math.Round(avgD);
            return new AverageResult ()
            {
                Average = avg,
                DateStamp = DateTime.Now
            };
        }

        //Process good votes as quickly as we can and return the current average.
        public Task ProcessGoodVote(Vote vote)
        {
            return Task.Factory.StartNew(() =>
            {
                if (vote.VoteValue < 0)
                {
                    //Add the vote to the local store of all votes
                    _Votes.Add(vote);

                    var avg = GetAverageResult();

                    VoteProcessed processed = new VoteProcessed()
                    {
                        VoterId = vote.VoterId,
                        ConnectionId = vote.ConnectionId,
                        ProcessedDateTime = DateTime.Now,
                        OriginalVoteValue = vote.VoteValue,
                        CurrentAverage = avg.Average
                    };

                    //Publish the latest average vote value to the bus
                    BusHost.Publish(avg, BusTopic.NewAverageResult);

                    //publish a message saying that the message was processed
                    BusHost.Publish(processed, BusTopic.VoteProcessed);
                }
            });
        }

        //Process bad votes in out own time..
        public Task ProcessBadVote(Vote vote)
        {
            return Task.Factory.StartNew(() =>
            {
                if (vote.VoteValue >= 0)
                {
                    //Bad votes take longer to vote.
                    Thread.Sleep(2000);

                    //Ever 4th bad vote gets "lost"
                    if (!(_Votes.Count % 4 == 0))
                    {
                        //Add the vote to the local store of all votes
                        _Votes.Add(vote);
                    }

                    var avg = GetAverageResult();

                    VoteProcessed processed = new VoteProcessed()
                    {
                        VoterId = vote.VoterId,
                        ConnectionId = vote.ConnectionId,
                        ProcessedDateTime = DateTime.Now,
                        OriginalVoteValue = vote.VoteValue,
                        CurrentAverage = avg.Average
                    };

                    //Publish the latest average vote value to the bus
                    BusHost.Publish(avg, BusTopic.NewAverageResult);

                    //publish a message saying 
                    BusHost.Publish(processed, BusTopic.VoteProcessed);
                }
            });
        }
    }
}
