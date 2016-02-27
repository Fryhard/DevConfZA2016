using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fryhard.DevConfZA2016.Web.State
{
    public static class VotingState
    {

        public static void Init ()
        {
            CurrentAverage = 0;
            LastUpdated = DateTime.Now;
        }

        public static int CurrentAverage { get; set; }

        public static DateTime LastUpdated { get; set; }
    }
}