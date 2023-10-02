namespace TollFeeCalculator.Services.FeeCalculators
{
    public class ElectricVehicleFeeCalculator : IFeeCalculator
    {

        private const int _lowFee = 2;
        private const int _mediumLowFee = 3;
        private const int _mediumFee = 4;
        private const int _mediumHighFee = 5;
        private const int _noFee = 0;

        public int CalculateIntervalFee(DateTime start, DateTime end)
        {
            return Math.Max(GetIndividualFee(start), GetIndividualFee(end));
        }

        public int GetIndividualFee(DateTime date)
        {
            return CalculateFeeBasedOnTime(date.Hour, date.Minute);

        }

        // this can be moved to its own class and use a strategy pattern so that we can change the logic without changing the code inside the method 
       
        private int CalculateFeeBasedOnTime(int hour, int minute)
        {
            return hour switch
            {
                6 when minute < 30 => _lowFee,
                6 => _mediumLowFee,
                7 => _mediumHighFee,
                8 when minute < 30 => _mediumHighFee,
                8 or 9 or 10 or 11 or 12 or 13 or 14 => _mediumFee,
                15 when minute < 30 => _mediumHighFee,
                15 or 16 => _mediumHighFee,
                17 => _mediumHighFee,
                18 when minute < 30 => _mediumLowFee,
                _ => _noFee
            };
        }
    }
}
