using System.Net.Http;
using System.Text.Json;
using MeteoPDF.Models;

namespace MeteoPDF.Services
{
    public class WeatherService
    {
        private HttpClient _http;

        public WeatherService()
        {
            _http = new HttpClient();
        }

        public async Task<WeatherResponse> GetWeatherAsync()
        {
            string url =
                "https://api.open-meteo.com/v1/forecast" +
                "?latitude=40.45" +
                "&longitude=-3.47" +
                "&hourly=temperature_2m,precipitation_probability" +
                "&forecast_days=7";

            string json = await _http.GetStringAsync(url);

            WeatherResponse result = JsonSerializer.Deserialize<WeatherResponse>(json);

            return result;
        }
    }
}
