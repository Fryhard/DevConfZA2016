using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Vote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoteId { get; set; }

        public string UserAgent { get; set; }

        public string IpAddress { get; set; }

        public string ConnectionId { get; set; }

        public string VoterId { get; set; }

        public int VoteValue { get; set; }

        public DateTime VoteDateTime { get; set; }
    }
}