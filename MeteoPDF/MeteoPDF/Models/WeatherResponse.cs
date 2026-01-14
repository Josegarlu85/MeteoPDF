namespace MeteoPDF.Models
{
    //clase que mostrara un item del tiempo, los que se añadiran al collectionview
    public class WeatherItem
    {
        public string Hora { get; set; }
        public string Temperatura { get; set; }
        public string ProbabilidadLluvia { get; set; }
    }
    //clase que representara la respuesta hora de la api del tiempo
    public class WeatherResponse
    {
        public Hourly hourly { get; set; }
    }
    //clase que representara los datos de hora de la api api-meteo
    public class Hourly
    {
        public List<string> time { get; set; }
        public List<double> temperature_2m { get; set; }
        public List<int> precipitation_probability { get; set; }
    }
}
