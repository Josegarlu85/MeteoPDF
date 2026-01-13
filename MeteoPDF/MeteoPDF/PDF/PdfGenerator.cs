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
        private WeatherResponse _data;

        public PdfGenerator(WeatherResponse data)
        {
            _data = data;
        }

        public byte[] Generate()
        {
            IDocument document = Document.Create(
                delegate (IDocumentContainer container)
                {
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

                                    column.Item().Table(
                                        delegate (TableDescriptor table)
                                        {
                                            // Definición de columnas
                                            table.ColumnsDefinition(
                                                delegate (TableColumnsDefinitionDescriptor columns)
                                                {
                                                    columns.RelativeColumn(); // Hora
                                                    columns.RelativeColumn(); // Temperatura
                                                    columns.RelativeColumn(); // Probabilidad lluvia
                                                });

                                            // Fila de cabecera
                                            table.Cell().Element(CellStyle).Text("Hora");
                                            table.Cell().Element(CellStyle).Text("Temp (°C)");
                                            table.Cell().Element(CellStyle).Text("Lluvia (%)");

                                            // Filas de datos
                                            if (_data != null &&
                                                _data.hourly != null &&
                                                _data.hourly.time != null &&
                                                _data.hourly.temperature_2m != null &&
                                                _data.hourly.precipitation_probability != null)
                                            {
                                                int count = Math.Min(
                                                    _data.hourly.time.Count,
                                                    Math.Min(
                                                        _data.hourly.temperature_2m.Count,
                                                        _data.hourly.precipitation_probability.Count
                                                    )
                                                );

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

        private QuestContainer CellStyle(QuestContainer container)
        {
            return container
                .Padding(5)
                .BorderBottom(1)
                .BorderColor(ColorsAlias.Grey.Lighten2);
        }
    }
}
