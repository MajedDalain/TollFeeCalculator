using Nager.Date.Model;

namespace TollFeeCalculator.Services.TollFreeConditions
{
    public class TollFreeDate : ITollFreeCondition
    {
        private readonly HashSet<DateTime> _publicHolidays;
        private readonly HashSet<DayOfWeek> _tollFreeDaysOfWeek;
        private readonly HashSet<int> _tollFreeMonths;

        public TollFreeDate(IEnumerable<DateTime> publicHolidays,
                            IEnumerable<DayOfWeek> tollFreeDaysOfWeek,
                            IEnumerable<int> tollFreeMonths)
        {
            _publicHolidays = new HashSet<DateTime>(publicHolidays);
            _tollFreeDaysOfWeek = new HashSet<DayOfWeek>(tollFreeDaysOfWeek);
            _tollFreeMonths = new HashSet<int>(tollFreeMonths);
        }

        // need to refactor since vehicle is not used here!
        public bool IsTollFree(Vehicle vehicle, DateTime date)
        {
            return _tollFreeDaysOfWeek.Contains(date.DayOfWeek) ||
                 _tollFreeMonths.Contains(date.Month) ||
                 _publicHolidays.Contains(date.Date);
        }
    }

}
