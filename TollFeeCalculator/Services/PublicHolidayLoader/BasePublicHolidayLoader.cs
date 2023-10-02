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

        // this can be improved to two methods, one for fetching and one for deserializing 
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
                // maybe this is not needed! since it is used in TollFreeDate and converted into 
                // a hashset which solves the problem of immediate execution of IEnumberable 
            }

            return PublicHolidays;
        }
    }

}
