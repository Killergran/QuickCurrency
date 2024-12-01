using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace QuickCurrency
{


    public class MainViewModel
    {

        public ObservableCollection<CurrencyExchange> CurrencyExchanges { get; set; }
        private readonly CurrencyService _currencyService;

        public MainViewModel()
        {
            CurrencyExchanges = new ObservableCollection<CurrencyExchange>();
            _currencyService = new CurrencyService();
            LoadCurrencyRates();
        }

        private async void LoadCurrencyRates()
        {
            var currency = "SEK";
            var apiKey = GetApiKey(); // Replace with your actual API key
            var response = await _currencyService.GetCurrencyRatesAsync(apiKey, currency);

            if (response != null && response.Result == "success")
            {
                var currenciesToShow = new List<string> { "USD", "EUR", "CNY" };


                var filteredRates = response.Conversion_Rates
                    .Where(rate => currenciesToShow.Contains(rate.Key))
                    .ToDictionary(rate => rate.Key, rate => rate.Value);

                foreach (var rate in filteredRates)
                {
                    decimal invertedRate = 1 / rate.Value;
                    CurrencyExchanges.Add(new CurrencyExchange
                    {

                        FromCurrency = currency,
                        ToCurrency = rate.Key,
                        ExchangeRate = decimal.Parse(invertedRate.ToString("F2"))
                    });
                }
            }
            currency = "USD";
            response = await _currencyService.GetCurrencyRatesAsync(apiKey, currency);


            if (response != null && response.Result == "success")
            {
                var currenciesToShow = new List<string> { "RUB" };

                var filteredRates = response.Conversion_Rates
                    .Where(rate => currenciesToShow.Contains(rate.Key))
                    .ToDictionary(rate => rate.Key, rate => rate.Value);

                foreach (var rate in filteredRates)
                {
                    CurrencyExchanges.Add(new CurrencyExchange
                    {

                        FromCurrency = currency,
                        ToCurrency = rate.Key,
                        ExchangeRate = decimal.Parse(rate.Value.ToString("F2"))
                    });
                }
            }
        }

        private static string GetApiKey()
        {
            StreamReader apiKeyFile = new System.IO.StreamReader("apikey.txt");
            return apiKeyFile.ReadToEnd();

        }
    }
}

