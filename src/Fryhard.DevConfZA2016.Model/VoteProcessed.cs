using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class VoteProcessed
    {
        public string ConnectionId { get; set; }

        public string VoterId { get; set; }

        public int OriginalVoteValue { get; set; }

        public int CurrentAverage { get; set; }

        public DateTime ProcessedDateTime { get; set; }
    }
}