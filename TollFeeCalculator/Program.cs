
namespace TollFeeCalculator
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Calculating The Car fees ........");

            var dateArray = GenerateTestDates();
            var publicHolidayLoader = InitializePublicHolidayLoader();

            var publicHolidays = await publicHolidayLoader.LoadPublicHolidaysAsync();

            CalculateElectricVehicleFees(dateArray, publicHolidays);
            CalculateVehicleAndDaysFees(dateArray, publicHolidays);

            Console.ReadLine();
        }

        private static DateTime[] GenerateTestDates()
        {
            return new[]
            {
                new DateTime(2023, 09, 12, 08, 10, 0),
                new DateTime(2023, 09, 12, 08, 15, 0),
                new DateTime(2023, 09, 12, 08, 17, 0),
            };
        }

        private static IPublicHolidayLoader InitializePublicHolidayLoader()
        {
            var httpClient = new HttpClient();
            var jsonSerializer = new JsonSerializerWrapper();
            var apiUrl = "https://date.nager.at/api/v3";
            return new SwedenPublicHolidayLoader(httpClient, jsonSerializer, apiUrl);
        }

        private static void CalculateElectricVehicleFees(DateTime[] dateArray, List<DateTime> publicHolidays)
        {
            var tollFreeChecker = CreateTollFreeCheckerForDays(publicHolidays);
            var tollCalculator = new TollCalculator(tollFreeChecker, new ElectricVehicleFeeCalculator());
            var totalFee = tollCalculator.GetTollFee(new ElectricVehicle(), dateArray);

            Console.WriteLine($"The total fee for the day is: {totalFee}");
            Console.WriteLine($"-------------------------------------------------------");
        }

        private static void CalculateVehicleAndDaysFees(DateTime[] dateArray, List<DateTime> publicHolidays)
        {
            var tollFreeChecker = CreateTollFreeCheckerForVehicleAndDays(publicHolidays);
            var tollCalculator = new TollCalculator(tollFreeChecker, new DefaultFeeCalculator());
            var totalFee = tollCalculator.GetTollFee(new Car(), dateArray);

            Console.WriteLine($"The total fee for the day and vehicle is: {totalFee}");
            Console.WriteLine($"-------------------------------------------------------");
        }

        private static ITollFreeChecker CreateTollFreeCheckerForDays(List<DateTime> publicHolidays)
        {
            return new DefaultTollFreeChecker(new List<ITollFreeCondition>
            {
                new TollFreeDate(publicHolidays, new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday }, new List<int> { 7 })
            });
        }

        private static ITollFreeChecker CreateTollFreeCheckerForVehicleAndDays(List<DateTime> publicHolidays)
        {
            return new DefaultTollFreeChecker(new List<ITollFreeCondition>
            {
                new TollFreeDate(publicHolidays, new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday }, new List<int> { 7 }),
                new TollFreeVehicle()
            });
        }
    }
}