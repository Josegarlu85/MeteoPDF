using System.Net.Http;
using System.Text.Json;
using MeteoPDF.Models;

namespace MeteoPDF.Services
{

    public class WeatherService
    {
        //Cliente http para hacer peticiones
        private HttpClient _http;

        public WeatherService()
        {
            //Inicializo el cliente http
            _http = new HttpClient();
        }
        //Metodo que obtiene los datos del tiempo de la api open-meteo
        public async Task<WeatherResponse> GetWeatherAsync()
        {
            //URL de la api con lo que le pido a esta
            string url =
                "https://api.open-meteo.com/v1/forecast" +
                "?latitude=40.45" +
                "&longitude=-3.47" +
                "&hourly=temperature_2m,precipitation_probability" +
                "&forecast_days=7";
            //obtengo el json de la api con la url guardada
            string json = await _http.GetStringAsync(url);
            //deserializo el json a un objeto WeatherResponse
            WeatherResponse result = JsonSerializer.Deserialize<WeatherResponse>(json);

            return result;
        }
    }
}

