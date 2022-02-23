using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using VacationHire.VehicleRentals.Models.Configuration;
using VacationHire.VehicleRentals.Services.Interfaces;

namespace VacationHire.VehicleRentals.Services
{
    public class ConversionService : IConversionService
    {
        private readonly IOptions<CurrencyLayerConfiguration> _currencyLayerConfiguration;

        public ConversionService(IOptions<CurrencyLayerConfiguration> currencyLayerConfiguration)
        {
            _currencyLayerConfiguration = currencyLayerConfiguration;
        }

        public async Task<decimal> ConvertAmountFromUsdTo(string newCurrency, decimal amount)
        {
            var externalEndpoint = GetExternalEndpoint();

            HttpClient client = new();
            var httpResponseMessage = await client.GetAsync(externalEndpoint);

            var usdToCurrency = await GetCurrencyValue(newCurrency, httpResponseMessage);

            return usdToCurrency * amount;
        }

        private string GetExternalEndpoint()
        {
            var config = _currencyLayerConfiguration.Value;

            return $"{config.BaseUrl}{config.Endpoint}?access_key={config.AccessKey}";
        }

        private static async Task<decimal> GetCurrencyValue(string newCurrency, HttpResponseMessage httpReponseMessage)
        {
            var responseContent = await httpReponseMessage.Content.ReadAsStringAsync();

            var parsed = JObject.Parse(responseContent);

            var usdToCurrency = parsed
                .SelectToken($"quotes.USD{newCurrency.ToUpper()}")
                .Value<decimal>();

            return usdToCurrency;
        }
    }
}