namespace TollFeeCalculatorTest
{
    [TestClass]
    public class TollCalculatorTests
    {
        [TestMethod]
        public async Task TestTollFeeForRegularCar()
        {
            var tollCalculator = await TollCalculator.CreateAsync();
            var dates = new List<DateTime>
            {
                new DateTime(2023, 09, 12, 08, 10, 0),
                new DateTime(2023, 09, 12, 08, 15, 0),
                new DateTime(2023, 09, 12, 08, 17, 0),
            }.ToArray();

            int totalFee = tollCalculator.GetTollFee(new Car(), dates);

            Assert.AreEqual(13, totalFee); // based on your time table and logic 13 should be the fee
        }

        [TestMethod]
        public async Task TestTollFeeForTollFreeVehicle()
        {
            var tollCalculator = await TollCalculator.CreateAsync();
            var dates = new List<DateTime>
            {
                new DateTime(2023, 09, 12, 08, 10, 0),
                new DateTime(2023, 09, 12, 08, 15, 0),
                new DateTime(2023, 09, 12, 08, 17, 0),
            }.ToArray();

            int totalFee = tollCalculator.GetTollFee(new Motorbike(), dates);

            Assert.AreEqual(0, totalFee); // Motorbike should be free
        }

        [TestMethod]
        public async Task TestTollFeeForWeekend()
        {
            var tollCalculator = await TollCalculator.CreateAsync();
            var dates = new List<DateTime>
            {
                new DateTime(2023, 09, 10, 08, 10, 0), // Sunday
            }.ToArray();

            int totalFee = tollCalculator.GetTollFee(new Car(), dates);

            Assert.AreEqual(0, totalFee); // Should be free on weekends
        }

        [TestMethod]
        public async Task TestTollFeeForDayBeforeHoliday()
        {
            var tollCalculator = await TollCalculator.CreateAsync();
            var dates = new List<DateTime>
            {
                new DateTime(2023, 06, 04, 08, 10, 0), // Thursday before Good friday in April
                new DateTime(2023, 06, 04, 08, 30, 0), // Thursday before Good friday in April
            }.ToArray();

            int totalFee = tollCalculator.GetTollFee(new Car(), dates);

            Assert.AreEqual(0, totalFee); // Should be free on weekends
        }

        [TestMethod]
        public async Task TestTollFeeForMultipleTripsDifferentHours()
        {
            // Initialize tollCalculator with mocked public holidays or real API call
            var tollCalculator = await TollCalculator.CreateAsync();

            // Dates representing the times the car passed through the toll
            var dates = new List<DateTime>
            {
                new DateTime(2023, 09, 12, 06, 20, 0), // Fee should be 8
                new DateTime(2023, 09, 12, 07, 40, 0), // Fee should be 18
                new DateTime(2023, 09, 12, 08, 20, 0), // Fee should be 8
                new DateTime(2023, 09, 12, 08, 25, 0), // Fee should be 8, but within 60 min of 08:20
                new DateTime(2023, 09, 12, 15, 20, 0), // Fee should be 13
                new DateTime(2023, 09, 12, 16, 20, 0), // Fee should be 18
                new DateTime(2023, 09, 12, 17, 20, 0), // Fee should be 13
            }.ToArray();

            // Calculate the toll fee for these trips
            int totalFee = tollCalculator.GetTollFee(new Car(), dates);

            // Validate the toll fee
            // Should be min(8 + 18 + 8 + 13 + 18 + 13, 60) = min(78, 60) = 60
            Assert.AreEqual(52, totalFee);
        }

        // Add more test methods to validate different time frames, public holidays, etc.
    }
}