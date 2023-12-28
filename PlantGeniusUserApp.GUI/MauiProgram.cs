using Microsoft.Extensions.Logging;

namespace PlantGeniusUserApp.GUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            //With this handler we will be able to place images and text of the <Shell.TitleView> to the edge. Before there was always a margin there.
            Microsoft.Maui.Handlers.ToolbarHandler.Mapper.AppendToMapping("CustomNavigationView", (handler, view) =>
            {
                #if ANDROID
                handler.PlatformView.ContentInsetStartWithNavigation = 0;
                #endif
            }); Microsoft.Maui.Handlers.ToolbarHandler.Mapper.AppendToMapping("CustomNavigationView", (handler, view) =>
            {
                #if ANDROID
                handler.PlatformView.ContentInsetStartWithNavigation = 0;
                #endif
            });

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            #if DEBUG
    		    builder.Logging.AddDebug();
            #endif

            return builder.Build();
        }



    }
}
