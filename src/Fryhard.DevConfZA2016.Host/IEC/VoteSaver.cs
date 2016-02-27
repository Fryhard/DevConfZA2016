using Fryhard.DevConfZA2016.Common;
using log4net;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fryhard.DevConfZA2016.Host.IEC
{
    public static class VoteSaver
    {
        private static readonly ILog _Log = LogManager.GetLogger(typeof(Host));

        public static Task ProcessVote(Vote vote)
        {
            return Task.Factory.StartNew(() =>
            {
                //Save the vote to the database
                using (DevConfVoteContext db = new DevConfVoteContext())
                {
                    db.Votes.Add(vote);
                    db.SaveChanges();
                }
            });
        }
    }
}
