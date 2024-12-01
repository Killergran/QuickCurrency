using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuickCurrency
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ApiResponse> GetCurrencyRatesAsync(string apiKey, string currency)
        {
            var url = $"https://v6.exchangerate-api.com/v6/{apiKey}/latest/{currency}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync(); 
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var jsonSerialized = JsonSerializer.Deserialize<ApiResponse>(json, options);
                return jsonSerialized;
            }

            return null;
        }
    }
}