using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fryhard.DevConfZA2016.Model
{
    public class Vote
    {
        public string UserAgent { get; set; }
        public string IpAddress { get; set; }
        public string ConnectionId { get; set; }
        public int VoteValue { get; set; }
        public DateTime VoteDateTime { get; set; }
    }
}