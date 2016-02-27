using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fryhard.DevConfZA2016.Common
{
    public enum BusTopic
    {
        NewVote = 0,
        VoteProcessed = 1,
        NewAverageResult = 10,
    }
}
