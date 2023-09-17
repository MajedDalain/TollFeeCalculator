namespace TollFeeCalculator.Services.FeeCalculators
{
    public class DefaultFeeCalculator : IFeeCalculator
    {

        private const int _lowFee = 8;
        private const int _mediumFee = 13;
        private const int _highFee = 18;
        private const int _noFee = 0;

        public int CalculateIntervalFee(DateTime start, DateTime end)
        {
            // Calculate the maximum individual fee between the start and end time
            return Math.Max(GetIndividualFee(start), GetIndividualFee(end));
        }

        public int GetIndividualFee(DateTime date)
        {
            return CalculateFeeBasedOnTime(date.Hour, date.Minute);

        }

        private int CalculateFeeBasedOnTime(int hour, int minute)
        {
            // Determine fee based on time
            return hour switch
            {
                6 when minute < 30 => _lowFee,
                6 => _mediumFee,
                7 => _highFee,
                8 when minute < 30 => _mediumFee,
                8 or 9 or 10 or 11 or 12 or 13 or 14 => _lowFee,
                15 when minute < 30 => _mediumFee,
                15 or 16 => _highFee,
                17 => _mediumFee,
                18 when minute < 30 => _lowFee,
                _ => _noFee
            };
        }
    }

}
