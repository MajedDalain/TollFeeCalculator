using TollFeeCalculator.Utilities.JsonSerializer;

namespace TollFeeCalculator.Services.PublicHolidayLoader
{


    public abstract class BasePublicHolidayLoader : IPublicHolidayLoader
    {
        private readonly HttpClient _httpClient;
        private readonly IJsonSerializerWrapper _jsonSerializer; 
        private readonly string _apiUrl;
        protected abstract string CountryCode { get; }


        public BasePublicHolidayLoader(HttpClient httpClient, IJsonSerializerWrapper jsonSerializer, string apiUrl)
        {
            _httpClient = httpClient;
            _jsonSerializer = jsonSerializer;
            _apiUrl = apiUrl;
        }

        public async Task<List<DateTime>> LoadPublicHolidaysAsync()
        {
            var PublicHolidays = new List<DateTime>();
            using var response = await _httpClient.GetAsync($"{_apiUrl}/publicholidays/{DateTime.Now.Year}/{CountryCode}");

            if(response.IsSuccessStatusCode)
            {
                using var jsonStream = await response.Content.ReadAsStreamAsync();
                var customPublicHolidays = _jsonSerializer.Deserialize<CustomPublicHoliday[]>(jsonStream);
                if(customPublicHolidays != null)
                    PublicHolidays = customPublicHolidays.Select(h => h.Date).ToList();
            }

            return PublicHolidays;
        }
    }

}
