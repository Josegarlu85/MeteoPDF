using QuestPDF.Infrastructure;

namespace MeteoPDF
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            //Indica que tipo de licencia usare a QuestPDF
            QuestPDF.Settings.License = LicenseType.Community;

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            return builder.Build();
        }
    }
}
