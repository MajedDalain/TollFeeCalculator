using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator.Utilities.TollFeeInterval
{
    public class TollFeeInterval
    {
        public TimeSpan Start { get; }
        public TimeSpan End { get; }
        public int Fee { get; }

        public TollFeeInterval(TimeSpan start, TimeSpan end, int fee)
        {
            Start = start;
            End = end;
            Fee = fee;
        }

        public bool IsWithinInterval(TimeSpan time)
        {
            return time >= Start && time <= End;
        }
    }
}
