namespace TollFeeCalculator.Services.PublicHolidayLoader
{
    public interface IPublicHolidayLoader
    {
        Task<List<DateTime>> LoadPublicHolidaysAsync();
    }

}
