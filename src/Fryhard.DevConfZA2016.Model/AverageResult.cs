using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class AverageResult
    {
        public int Average { get; set; }
        
        public DateTime DateStamp { get; set; }

        public int CountBigSmile { get; set; }
        public int CountSmallSmile { get; set; }
        public int CountNoSmile { get; set; }
        public int CountSmallFrown { get; set; }
        public int CountBigFrown { get; set; }

        public int TotalCount { get; set; }

        public int LostNoSmile { get; set; }
        public int LostSmallFrown { get; set; }
        public int LostBigFrown { get; set; }



        public int CountLost { get; set; }
    }
}