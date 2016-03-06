using Fryhard.DevConfZA2016.Common;
using log4net;
using Model;
using System;
using System.Collections.Concurrent;
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

        private ConcurrentBag<Vote> _Votes { get; set; }

        public VoteProcessor ()
        {
            _Votes = new ConcurrentBag<Vote>();
        }

        public AverageResult GetAverageResult ()
        {
            AverageResult ret = new AverageResult ();

            if (_Votes.Count != 0)
            {
                int total = _Votes.Where(v => !v.VoteLost)
                                  .Sum(v => v.VoteValue);

                ret.TotalCount = _Votes.Where(v => !v.VoteLost)
                                  .Count();

                var aggregate = _Votes.Where(v => !v.VoteLost)
                                      .GroupBy(v => v.VoteValue)
                                      .Select(g => new { Key = g.Key, Num = g.Count() });

                ret.CountBigSmile = aggregate.Where (a => a.Key == -2)
                                             .Select (a => a.Num)
                                             .FirstOrDefault();

                ret.CountSmallSmile = aggregate.Where (a => a.Key == -1)
                                             .Select (a => a.Num)
                                             .FirstOrDefault();

                ret.CountNoSmile = aggregate.Where (a => a.Key == 0)
                                             .Select (a => a.Num)
                                             .FirstOrDefault();

                ret.CountSmallFrown = aggregate.Where (a => a.Key == 1)
                                             .Select (a => a.Num)
                                             .FirstOrDefault();

                ret.CountBigFrown = aggregate.Where (a => a.Key == 2)
                                             .Select (a => a.Num)
                                             .FirstOrDefault();

                ret.CountLost = _Votes.Where(v => v.VoteLost)
                                      .Count();


                var lostAggregate = _Votes.Where(v => v.VoteLost)
                                          .GroupBy(v => v.VoteValue)
                                          .Select(g => new { Key = g.Key, Num = g.Count() });

                ret.LostNoSmile = lostAggregate.Where(a => a.Key == 0)
                                             .Select(a => a.Num)
                                             .FirstOrDefault();

                ret.LostSmallFrown = lostAggregate.Where(a => a.Key == 1)
                                             .Select(a => a.Num)
                                             .FirstOrDefault();

                ret.LostBigFrown = lostAggregate.Where(a => a.Key == 2)
                                             .Select(a => a.Num)
                                             .FirstOrDefault();


                decimal avgDec = (decimal)total / (decimal)ret.TotalCount;

                _Log.Debug("Total = " + total + ". Votes = " + ret.TotalCount + ". Avg = " + avgDec);

                ret.Average = (int) Math.Round(avgDec);
                
                ret.DateStamp = DateTime.Now;
            }

            return ret;
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
                    //Bad votes take longer to count.
                    Thread.Sleep(3000);

                    //One in every 4 bad votes get lost.
                    if ((_Votes.Count +1) % 4 == 0)
                    {
                        //Add the vote to the local store of all votes
                        vote.VoteLost = true;
                    }
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

                    //publish a message saying 
                    BusHost.Publish(processed, BusTopic.VoteProcessed);
                }
            });
        }

        public void Reset ()
        {
            _Votes = new ConcurrentBag<Vote>();
        }
    }
}
