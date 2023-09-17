namespace TollFeeCalculator.Services.TollFreeConditions
{
    public interface ITollFreeCondition
    {
        bool IsTollFree(Vehicle vehicle, DateTime date);
    }

}
