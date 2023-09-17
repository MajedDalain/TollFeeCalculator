namespace TollFeeCalculator.Services.FeeCalculators
{
    public interface IFeeCalculator
    {
        int CalculateIntervalFee(DateTime start, DateTime end);
        int GetIndividualFee(DateTime date);
    }

}
