﻿using TollFeeCalculator.Services.FeeCalculators;
using TollFeeCalculator.Services.TollFreeChecker;

namespace TollFeeCalculator.Services
{

    public class TollCalculator
    {
        private const int MaxDailyFee = 60;  // Max fee for a day
        private readonly ITollFreeChecker _tollFreeChecker;
        private readonly IFeeCalculator _feeCalculator;
        public TollCalculator(ITollFreeChecker tollFreeChecker, IFeeCalculator feeCalculator)
        {
            _tollFreeChecker = tollFreeChecker ?? throw new ArgumentNullException(nameof(tollFreeChecker));
            _feeCalculator = feeCalculator ?? throw new ArgumentNullException(nameof(feeCalculator));
        }
        public int GetTollFee(Vehicle vehicle, DateTime[] dates)
        {
            return IsTollFree(vehicle, dates) ? 0 : CalculateTollFee(dates);
        }

        // in case we want to refactor more, we can add this logic to another class that only checks for IsAnyDateTollFree
        private bool IsTollFree(Vehicle vehicle, DateTime[] dates)
        {
            return  dates.Any(date => _tollFreeChecker.IsTollFree(vehicle,date));
        }


        // also if we want to refactor more, we can split these 3 methods into another class that handles the calculation logic 
        private int CalculateTollFee(DateTime[] dates)
        {
            DateTime currentIntervalStart = dates[0]; 
            int totalFee = 0;
            foreach(DateTime date in dates)
            {
                if(IsWithinOneHour(date, currentIntervalStart))
                {
                    totalFee = UpdateFeeForCurrentInterval(currentIntervalStart, date, totalFee);
                }
                else
                {
                    totalFee += _feeCalculator.GetIndividualFee(date);
                    currentIntervalStart = date;
                }
            }
            return Math.Min(totalFee, MaxDailyFee);
        }

        private bool IsWithinOneHour(DateTime date, DateTime currentIntervalStart)
        {
            return (date - currentIntervalStart).TotalMinutes <= 60;
        }

        private int UpdateFeeForCurrentInterval(DateTime intervalStart, DateTime currentDateTime, int currentMaxFee)
        {
            int tempFee = _feeCalculator.CalculateIntervalFee(intervalStart, currentDateTime);
            return Math.Max(tempFee, currentMaxFee);
        }

    }

}
