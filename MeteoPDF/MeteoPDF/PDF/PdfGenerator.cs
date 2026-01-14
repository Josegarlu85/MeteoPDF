using System;
using MeteoPDF.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

// Alias para evitar conflicto con IContainer de MAUI
using QuestContainer = QuestPDF.Infrastructure.IContainer;
// Alias para evitar conflicto con Colors de MAUI
using ColorsAlias = QuestPDF.Helpers.Colors;

namespace MeteoPDF.Pdf
{
    public class PdfGenerator
    {
        //Guarda los datos del tiempo 
        private WeatherResponse _data;
        //Constructor que recibe los datos del tiempo
        public PdfGenerator(WeatherResponse data)
        {
            _data = data;
        }
        //metodo que generara el pdf y lo devuelve en un array de bytes
        public byte[] Generate()
        {   //crea el documento pdf, usa metdos de questpdf
            IDocument document = Document.Create(
                delegate (IDocumentContainer container)
                {
                    //a partir de aqui define como sera la pagina
                    container.Page(
                        delegate (PageDescriptor page)
                        {
                            page.Margin(20);

                            page.Content().Column(
                                delegate (ColumnDescriptor column)
                                {
                                    column.Item().Text("Informe Meteorológico").FontSize(22).Bold();
                                    column.Item().Text("Previsión para los próximos 7 días en Torrejón de Ardoz (Madrid)").FontSize(16);
                                    column.Item().Text($"Datos obtenidos de Open‑Meteo el {DateTime.Now:dd/MM/yyyy} a las {DateTime.Now:HH:mm}").FontSize(12).Italic();
                                    column.Item().Text("");
                                    //crea la tabla que mostrara los datos
                                    column.Item().Table(
                                        delegate (TableDescriptor table)
                                        {
                                            // Definición de columnas, una por cada dato
                                            table.ColumnsDefinition(
                                                delegate (TableColumnsDefinitionDescriptor columns)
                                                {
                                                    columns.RelativeColumn(); // Hora
                                                    columns.RelativeColumn(); // Temperatura
                                                    columns.RelativeColumn(); // Probabilidad lluvia
                                                });

                                            // Fila con los titulos de cabecera
                                            table.Cell().Element(CellStyle).Text("Hora");
                                            table.Cell().Element(CellStyle).Text("Temp (°C)");
                                            table.Cell().Element(CellStyle).Text("Lluvia (%)");


                                            if (_data != null &&
                                                _data.hourly != null &&
                                                _data.hourly.time != null &&
                                                _data.hourly.temperature_2m != null &&
                                                _data.hourly.precipitation_probability != null)
                                            {
                                                //Cuantas filas se mostraran
                                                int count = Math.Min(
                                                    _data.hourly.time.Count,
                                                    Math.Min(
                                                        _data.hourly.temperature_2m.Count,
                                                        _data.hourly.precipitation_probability.Count
                                                    )
                                                );
                                                //Bucle que va añadiendo las filas con los datos a la tabla
                                                for (int i = 0; i < count; i++)
                                                {
                                                    table.Cell().Element(CellStyle).Text(_data.hourly.time[i]);
                                                    table.Cell().Element(CellStyle).Text(_data.hourly.temperature_2m[i].ToString("F1"));
                                                    table.Cell().Element(CellStyle).Text(_data.hourly.precipitation_probability[i].ToString());
                                                }
                                            }
                                        });
                                });
                        });
                });

            return document.GeneratePdf();
        }
        //metodo para aplicar un estilo uniforme
        private QuestContainer CellStyle(QuestContainer container)
        {
            return container
                .Padding(5)
                .BorderBottom(1)
                .BorderColor(ColorsAlias.Grey.Lighten2);
        }
    }
}

