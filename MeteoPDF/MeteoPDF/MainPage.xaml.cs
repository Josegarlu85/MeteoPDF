using MeteoPDF.Models;
using MeteoPDF.Pdf;
using MeteoPDF.Services;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using System.Collections.ObjectModel;

namespace MeteoPDF
{
    public partial class MainPage : ContentPage
    {
        private WeatherResponse _weatherData;
        private WeatherService _service;

        public ObservableCollection<WeatherItem> WeatherItems { get; set; }

        public MainPage()
        {
            InitializeComponent();
            _service = new WeatherService();
            WeatherItems = new ObservableCollection<WeatherItem>();
            BindingContext = this;
        }
        //MEtodo usado al pulsar el boton que obtiene los datos del tiempo
        private async void BtnGetWeather_Clicked(object sender, EventArgs e)
        {
            LblStatus.Text = "Obteniendo datos...";

            _weatherData = await _service.GetWeatherAsync();
            //Si los datos no son nulos..
            if (_weatherData != null &&
                _weatherData.hourly != null &&
                _weatherData.hourly.time != null &&
                _weatherData.hourly.temperature_2m != null &&
                _weatherData.hourly.precipitation_probability != null)
            {
                //..limpiamos el collectionview..
                WeatherItems.Clear();
                //..y añadimos los nuevos datos que sacamos de la api
                int count = Math.Min(
                    _weatherData.hourly.time.Count,
                    Math.Min(
                        _weatherData.hourly.temperature_2m.Count,
                        _weatherData.hourly.precipitation_probability.Count
                    )
                );
                //bucle que va añadiendo los datos al collectionview weatheritems con los weatheritem individuales
                for (int i = 0; i < count; i++)
                {
                    WeatherItems.Add(new WeatherItem
                    {
                        Hora = _weatherData.hourly.time[i],
                        Temperatura = _weatherData.hourly.temperature_2m[i].ToString("F1") + " °C",
                        ProbabilidadLluvia = _weatherData.hourly.precipitation_probability[i] + " %"
                    });
                }

                LblStatus.Text = "Datos de los proximos 7 dias";
                //Pone en true el boton de generar pdf para ahora si poder usarlo
                BtnGeneratePdf.IsEnabled = true;
            }
            else
            {
                LblStatus.Text = "Error al obtener datos.";
            }
        }
        //Metodo que salta al pulsar el boton de generar pdf
        private async void BtnGeneratePdf_Clicked(object sender, EventArgs e)
        {
            LblStatus.Text = "Generando PDF...";
            //Crea el generador de pdf llamandolo generator y pasandole los datos del tiempo
            PdfGenerator generator = new PdfGenerator(_weatherData);
            //genera el pdf y lo guarda en un array de bytes
            byte[] pdfBytes = generator.Generate();
            //Guarda el pdf en la ruta de appdata con el nombre informe_meteo.pdf
            string path = Path.Combine(FileSystem.AppDataDirectory, "informe_meteo.pdf");
            File.WriteAllBytes(path, pdfBytes);
            //Usa el share request async para compartir el pdf generado
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Informe meteorológico",
                File = new ShareFile(path)
            });
            //actualizamos por ultima vez lblStatus para seguir lo que hemos hecho
            LblStatus.Text = "PDF generado y listo para compartir.";
        }
    }
}
