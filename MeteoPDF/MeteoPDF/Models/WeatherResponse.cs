using System.Collections.Generic;

namespace MeteoPDF.Models
{
    // Modelo principal que representa la respuesta de Open‑Meteo
    public class WeatherResponse
    {
        public Hourly hourly { get; set; }
    }

    // Datos por horas lista de horas yde temperaturas
    public class Hourly
    {
        public List<string> time { get; set; }
        public List<double> temperature_2m { get; set; }
        public List<int> precipitation_probability { get; set; }
    }

    // Modelo para mostrar en la app el CollectionView
    namespace MeteoPDF.Models
    {
        public class WeatherItem
        {
            public string Hora { get; set; }
            public string Temperatura { get; set; }
            public string ProbabilidadLluvia { get; set; }
        }
    }
}

