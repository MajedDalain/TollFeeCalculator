using TollFeeCalculator.Utilities.JsonSerializer;

namespace TollFeeCalculator.Services.PublicHolidayLoader
{
    public class GermanyPublicHolidayLoader: BasePublicHolidayLoader
    {
        public GermanyPublicHolidayLoader(HttpClient httpClient, IJsonSerializerWrapper jsonSerializer, string apiUrl)
          : base(httpClient, jsonSerializer, apiUrl)
        {
        }
        protected override string CountryCode => "DE";
    }

}
