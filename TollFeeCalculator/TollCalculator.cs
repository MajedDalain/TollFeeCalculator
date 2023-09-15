using Nager.Date;
using Nager.Date.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;


namespace TollFeeCalculator
{

    public class CustomPublicHoliday
    {
        public DateTime Date { get; set; }
        // other properties that you need
    }

    public class TollCalculator
    {

        private List<DateTime> PublicHolidays { get; set; } = new List<DateTime>();

        public static async Task<TollCalculator> CreateAsync()
        {
            var tollCalculator = new TollCalculator();
            await tollCalculator.LoadPublicHolidaysAsync();
            return tollCalculator;
        }

        public TollCalculator() { }

        public async Task LoadPublicHolidaysAsync()
        {
            var jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync($"https://date.nager.at/api/v3/publicholidays/{DateTime.Now.Year}/SE");
            if(response.IsSuccessStatusCode)
            {
                using var jsonStream = await response.Content.ReadAsStreamAsync();
                var customPublicHolidays = System.Text.Json.JsonSerializer.Deserialize<CustomPublicHoliday[]>(jsonStream, jsonSerializerOptions);
                if (customPublicHolidays != null)
                    PublicHolidays = customPublicHolidays.Select(h => h.Date).ToList();
                
            }
        }


        /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total toll fee for that day
         */

        public  int GetTollFee(Vehicle vehicle, DateTime[] dates)
        {

            var tollFreeDates = dates.Select(d => IsTollFreeDate(d)).ToArray();

            if(IsTollFreeVehicle(vehicle) || tollFreeDates.Any(d => d))
                return 0;

            DateTime intervalStart = dates[0]; // first date for passing 
            int totalFee = 0;
            foreach(DateTime date in dates)
            {
                TimeSpan diff  = date - intervalStart;
                if(diff.TotalMinutes <= 60) 
                {
                    int tempFee = CalculateIntervalFee(intervalStart, date, vehicle);
                    if(tempFee > totalFee) totalFee = tempFee;
                }
                else
                {
                    totalFee += GetIndividualFee(date);
                    intervalStart = date;
                }
            }
            return Math.Min(totalFee, 60);
        }
        private int CalculateIntervalFee(DateTime start, DateTime end, Vehicle vehicle)
        {
            return Math.Max(GetIndividualFee(start), GetIndividualFee(end));
        }
        private int GetIndividualFee(DateTime date)
        {
            int hour = date.Hour;
            int minute = date.Minute;

            // Simplified for readability
            return hour switch
            {
                6 when minute < 30 => 8,
                6 => 13,
                7 => 18,
                8 when minute < 30 => 13,
                8 or 9 or 10 or 11 or 12 or 13 or 14 => 8,
                15 when minute < 30 => 13,
                15 or 16 => 18,
                17 => 13,
                18 when minute < 30 => 8,
                _ => 0
            };
        }

        private bool IsTollFreeVehicle(Vehicle vehicle)
        {
            return vehicle != null && Enum.TryParse(vehicle.GetVehicleType(), out TollFreeVehicles tollFree) && Enum.IsDefined(typeof(TollFreeVehicles), tollFree);
        }

        private bool IsTollFreeDate(DateTime date)
        {
            // Check if it's a weekend
            if(date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return true;

            // Check if it's July
            if(date.Month == 7)
                return true;

            // Check for public holidays 
            if(PublicHolidays.Any(holiday => holiday.Date == date))
                return true;

            // Check if the day before a public holiday
            if(PublicHolidays.Any( holiday => holiday.Date == date.AddDays(1)))
                return true;

            // No conditions met, not a toll-free date
            return false;

        } 

        private enum TollFreeVehicles
        {
            Motorbike = 0,
            Tractor = 1,
            Emergency = 2,
            Diplomat = 3,
            Foreign = 4,
            Military = 5
        }
    }
}
