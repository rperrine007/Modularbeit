using System.Globalization;

namespace PlantGeniusUserApp.GUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            // Set the culture to German
            CultureInfo ci = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}
