using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator.Utilities.TollFeeInterval
{
    public static class FeeService
    {
        private static readonly List<TollFeeInterval> _intervals = new List<TollFeeInterval>
    {
        new TollFeeInterval(new TimeSpan(6, 0, 0), new TimeSpan(6, 29, 59), 8),
        new TollFeeInterval(new TimeSpan(6, 30, 0), new TimeSpan(6, 59, 59), 13),
        new TollFeeInterval(new TimeSpan(7, 0, 0), new TimeSpan(7, 59, 59), 18),
        
    };

        public static int GetFeeForTime(int hour, int minute)
        {
            TimeSpan time = new TimeSpan(hour, minute, 0);
            foreach(var interval in _intervals)
            {
                if(interval.IsWithinInterval(time))
                {
                    return interval.Fee;
                }
            }

            return 0;  
        }
    }
}
