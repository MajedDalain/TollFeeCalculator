using Moq;
using TollFeeCalculator.Services.FeeCalculators;
using TollFeeCalculator.Services.PublicHolidayLoader;
using TollFeeCalculator.Services.TollFreeChecker;
using TollFeeCalculator.Services.TollFreeConditions;

namespace TollFeeCalculatorTest
{
    [TestClass]
    public class TollCalculatorTests
    {

        private static ITollFreeChecker? _tollFreeCheckerForDaysOnly;
        private static ITollFreeChecker? _tollFreeCheckerForVehicleAndDay;

        [ClassInitialize]
        public static void Init(TestContext context) 
        {
            var mockPublicHolidayLoader = new Mock<IPublicHolidayLoader>();
            mockPublicHolidayLoader.Setup(loader => loader.LoadPublicHolidaysAsync())
                         .ReturnsAsync(new List<DateTime> { new DateTime(2023, 1, 1), new DateTime(2023, 4, 2) });  // these are the holidays 

             _tollFreeCheckerForDaysOnly = new DefaultTollFreeChecker(new List<ITollFreeCondition>
                {
                    new TollFreeDate(mockPublicHolidayLoader.Object.LoadPublicHolidaysAsync().Result,
                                     new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday },
                                     new List<int> { 7 })
                });

            _tollFreeCheckerForVehicleAndDay = new DefaultTollFreeChecker(new List<ITollFreeCondition>
                {
                    new TollFreeDate(mockPublicHolidayLoader.Object.LoadPublicHolidaysAsync().Result,
                                     new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday },
                                     new List<int> { 7 }),
                    new TollFreeVehicle()
                });

        }

        [TestMethod]
        public void TestTollFeeForRegularCarWithDailyChecker()
        {
            Assert.IsNotNull(_tollFreeCheckerForDaysOnly, "_tollFreeCheckerForDaysOnly should not be null.");
            var tollCalculator = new TollCalculator(_tollFreeCheckerForDaysOnly, new DefaultFeeCalculator());
            var dates = new List<DateTime>
                {
                    new DateTime(2023, 09, 12, 08, 10, 0),
                    new DateTime(2023, 09, 12, 08, 15, 0),
                    new DateTime(2023, 09, 12, 08, 17, 0),
                }.ToArray();

        
            int totalFee = tollCalculator.GetTollFee(new Car(), dates);

            Assert.AreEqual(13, totalFee); // Based on your toll table and logic, 13 should be the fee
        }


        [TestMethod]
        public void TestTollFeeForTollFreeVehicleWithVehicleAndDayChecker()
        {
            Assert.IsNotNull(_tollFreeCheckerForVehicleAndDay, "_tollFreeCheckerForVehicleAndDay should not be null.");
            var tollCalculator = new TollCalculator(_tollFreeCheckerForVehicleAndDay, new DefaultFeeCalculator());
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
        public void TestTollFeeForFreeVehicleWithDailyChecker()
        {
            Assert.IsNotNull(_tollFreeCheckerForDaysOnly, "_tollFreeCheckerForDaysOnly should not be null.");
            var tollCalculator = new TollCalculator(_tollFreeCheckerForDaysOnly, new DefaultFeeCalculator());
            var dates = new List<DateTime>
            {
                new DateTime(2023, 09, 12, 08, 10, 0), // Tuesday
            }.ToArray();

            int totalFee = tollCalculator.GetTollFee(new Motorbike(), dates);

            Assert.AreEqual(13, totalFee); // even thoug it is a motorbik, but here we use another calss that checks only based on days 
        }

        [TestMethod]
        public void TestTollFeeForDayBeforeHoliday()
        {
            Assert.IsNotNull(_tollFreeCheckerForDaysOnly, "_tollFreeCheckerForDaysOnly should not be null.");
            var tollCalculator = new TollCalculator(_tollFreeCheckerForDaysOnly, new DefaultFeeCalculator());
            var dates = new List<DateTime>
            {
                new DateTime(2023, 04, 01, 08, 10, 0), // Thursday before Good friday in April
                new DateTime(2023, 04, 01, 08, 30, 0), // Thursday before Good friday in April
            }.ToArray();

            int totalFee = tollCalculator.GetTollFee(new Car(), dates);

            Assert.AreEqual(0, totalFee); // Should be free on weekends and a day before holiday
        }

        [TestMethod]
        public void TestTollFeeForMultipleTripsDifferentHours()
        {
            Assert.IsNotNull(_tollFreeCheckerForDaysOnly, "_tollFreeCheckerForDaysOnly should not be null.");
            var tollCalculator = new TollCalculator(_tollFreeCheckerForDaysOnly, new DefaultFeeCalculator());

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
            Assert.AreEqual(52, totalFee);
        }

    }
}