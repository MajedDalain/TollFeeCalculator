namespace TollFeeCalculator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var tollCalc = await TollCalculator.CreateAsync();

            var datesList = new List<DateTime>
            {
                new DateTime(2023, 09, 12, 08, 10, 0),
                new DateTime(2023, 09, 12, 08, 15, 0),
                new DateTime(2023, 09, 12, 08, 17, 0),
            };

          
            var dateArray = datesList.ToArray();
            var totalFee = tollCalc.GetTollFee(new Car(), dateArray);

            Console.WriteLine($"The total fee for the day is: {totalFee}");
            Console.ReadLine();
        }
    }
}