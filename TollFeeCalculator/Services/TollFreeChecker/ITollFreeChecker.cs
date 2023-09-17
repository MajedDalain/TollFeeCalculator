namespace TollFeeCalculator.Services.TollFreeChecker
{
    public interface ITollFreeChecker
    {
        bool IsTollFree(Vehicle vehicle, DateTime date);
    }

}
