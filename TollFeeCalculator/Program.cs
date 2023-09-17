using TollFeeCalculator.Models;
using TollFeeCalculator.Services.FeeCalculators;
using TollFeeCalculator.Services.PublicHolidayLoader;
using TollFeeCalculator.Services.TollFreeConditions;
using TollFeeCalculator.Utilities.JsonSerializer;

namespace TollFeeCalculator
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Calculating The Car fees ........");

            var datesList = new List<DateTime>
            {
                new DateTime(2023, 09, 12, 08, 10, 0),
                new DateTime(2023, 09, 12, 08, 15, 0),
                new DateTime(2023, 09, 12, 08, 17, 0),
            };
            var dateArray = datesList.ToArray();

            HttpClient httpClient = new HttpClient();
            IJsonSerializerWrapper jsonSerializer = new JsonSerializerWrapper();
            string apiUrl = "https://date.nager.at/api/v3";

            IPublicHolidayLoader publicHolidayLoader = new SwedenPublicHolidayLoader(httpClient, jsonSerializer, apiUrl);

            var publicHolidays = await publicHolidayLoader.LoadPublicHolidaysAsync();

            var tollFreeDaysOfWeek = new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday };
            var tollFreeMonths = new List<int> { 7 };  // July

            // here the toll fee are only based on day
            var tollFreeConditionsOnlyDays = new List<ITollFreeCondition>
            {
                new TollFreeDate(publicHolidays, tollFreeDaysOfWeek, tollFreeMonths)
            };

            var tollFreeCheckerOnlyDays = new DefaultTollFreeChecker(tollFreeConditionsOnlyDays);

            // this toll fee calculator will use another logic for the Electrical vehicles / less price for hours / 
            // and will apply the toll free only based on days 
            var tollCalculatorOnlyDays = new TollCalculator(tollFreeCheckerOnlyDays, new ElectricVehicleFeeCalculator());

            var totalFeeOnlyDays = tollCalculatorOnlyDays.GetTollFee(new ElectricVehicle(), dateArray);  // must check this and the vehicle passed 

            Console.WriteLine($"The total fee for the day is: {totalFeeOnlyDays}");  // should not be 0 for non holiday days 
            Console.WriteLine($"-------------------------------------------------------");


            // here toll free conditions are based on day and the type of vehicle
            var tollFreeConditionsVehicleAndDays = new List<ITollFreeCondition>
            {
                new TollFreeDate(publicHolidays, tollFreeDaysOfWeek, tollFreeMonths),
                new TollFreeVehicle()
            };

            var tollFreeCheckerVehicleAndDays = new DefaultTollFreeChecker(tollFreeConditionsVehicleAndDays);
         
            // this toll fee calculator will use the default logic. and the toll free based on both vehicle and day. 
            var tollCalculatorVehicleAndDays = new TollCalculator(tollFreeCheckerVehicleAndDays, new DefaultFeeCalculator());
            var totalFeeOnlyVehicleAndDays = tollCalculatorVehicleAndDays.GetTollFee(new Car(), dateArray);  // same here must check the vehilce passed. 

            Console.WriteLine($"The total fee for the day and vehicle is: {totalFeeOnlyVehicleAndDays}");  // has to be 0 since it is a Motorbike 



            Console.ReadLine();
        }
    }
}