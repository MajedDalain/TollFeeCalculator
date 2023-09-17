using TollFeeCalculator.Utilities.JsonSerializer;

namespace TollFeeCalculator.Services.PublicHolidayLoader
{
    public class SwedenPublicHolidayLoader : BasePublicHolidayLoader
    {
        public SwedenPublicHolidayLoader(HttpClient httpClient, IJsonSerializerWrapper jsonSerializer, string apiUrl)
          : base(httpClient, jsonSerializer, apiUrl)
        {
        }
        protected override string CountryCode => "SE";
    }

}
