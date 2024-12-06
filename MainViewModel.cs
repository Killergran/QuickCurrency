using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuickCurrency
{


    public class MainViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<CurrencyExchange> CurrencyExchanges { get; set; }
        private readonly CurrencyService _currencyService;
        
        private decimal _inputAmount;
        public decimal InputAmount
        {
            get => _inputAmount;
            set
            {
                if (_inputAmount != value)
                {
                    _inputAmount = value;
                    ConvertAmount(); 
                    OnPropertyChanged();
                    
                }
            }
        }

        private decimal _convertedAmount;
        public decimal ConvertedAmount
        {
            get => _convertedAmount;
            set
            {
                if (_convertedAmount != value)
                {
                    _convertedAmount = value;
                    OnPropertyChanged();
                }
            }
        }

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

        private void ConvertAmount()
        {
            if (CurrencyExchanges.Any())
            {
                var selectedExchangeRate = CurrencyExchanges.First().ExchangeRate; // Assuming the first exchange rate for simplicity
                ConvertedAmount = InputAmount * selectedExchangeRate;
            }
        }
        private static string GetApiKey()
        {
            try
            {
                string path = Path.Combine(AppContext.BaseDirectory, "ApiKey.txt");
                StreamReader apiKeyFile = new System.IO.StreamReader(path);
                return apiKeyFile.ReadToEnd();
            }
            catch (Exception)
            {
                return "YOUR_API_KEY";
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

