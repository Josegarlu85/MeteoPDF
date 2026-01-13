using MeteoPDF.Models;
using MeteoPDF.Models.MeteoPDF.Models;
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

        private async void BtnGetWeather_Clicked(object sender, EventArgs e)
        {
            LblStatus.Text = "Obteniendo datos...";

            _weatherData = await _service.GetWeatherAsync();

            if (_weatherData != null &&
                _weatherData.hourly != null &&
                _weatherData.hourly.time != null &&
                _weatherData.hourly.temperature_2m != null &&
                _weatherData.hourly.precipitation_probability != null)
            {
                WeatherItems.Clear();

                int count = Math.Min(
                    _weatherData.hourly.time.Count,
                    Math.Min(
                        _weatherData.hourly.temperature_2m.Count,
                        _weatherData.hourly.precipitation_probability.Count
                    )
                );

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
                BtnGeneratePdf.IsEnabled = true;
            }
            else
            {
                LblStatus.Text = "Error al obtener datos.";
            }
        }

        private async void BtnGeneratePdf_Clicked(object sender, EventArgs e)
        {
            LblStatus.Text = "Generando PDF...";

            PdfGenerator generator = new PdfGenerator(_weatherData);
            byte[] pdfBytes = generator.Generate();

            string path = Path.Combine(FileSystem.AppDataDirectory, "informe_meteo.pdf");
            File.WriteAllBytes(path, pdfBytes);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Informe meteorológico",
                File = new ShareFile(path)
            });

            LblStatus.Text = "PDF generado y listo para compartir.";
        }
    }
}
