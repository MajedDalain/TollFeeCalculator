using TollFeeCalculator.Services.TollFreeConditions;

namespace TollFeeCalculator.Services
{
    public interface ITollFreeChecker
    {
        bool IsTollFree(Vehicle vehicle, DateTime date);
    }


    public class DefaultTollFreeChecker: ITollFreeChecker
    {
 
        private readonly List<ITollFreeCondition> tollFreeConditions; 
                                                                                        
        public DefaultTollFreeChecker(List<ITollFreeCondition> tollFreeConditions)
        {
            this.tollFreeConditions = tollFreeConditions;
        }

        public bool IsTollFree(Vehicle vehicle, DateTime date)
        {
            return tollFreeConditions.Any(condition => condition.IsTollFree(vehicle, date));
        }


    }

}
