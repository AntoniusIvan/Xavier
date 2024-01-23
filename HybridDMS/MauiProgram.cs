using HybridDMS.Components.Data;
using HybridDMS.Components.Services;
using Microsoft.Extensions.Logging;

namespace HybridDMS
{
    public static class MauiProgram
    {
        private static string mobileBackendHost = "https://localhost:7224";
        internal static string mobileBackendCatalogBaseUrl = $"{mobileBackendHost}/catalog-api/";
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            builder.Services.AddHttpClient<CatalogService>(o => o.BaseAddress = new(mobileBackendCatalogBaseUrl));
            builder.Services.AddSingleton<TodoService>();
            //05:03 Why Connectivity is singleton? because we only need 1 instance of it in whole apps.
            builder.Services.AddSingleton<IConnectivity>(c =>
                Connectivity.Current);

            return builder.Build();
        }
    }
}
