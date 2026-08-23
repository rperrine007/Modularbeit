using System;
using System.Windows;
using PlantGenius.DAL;
using PlantGenius.GUI.Views;

namespace PlantGenius.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // DB-Kontext erstellen und Seed ausführen
            using var context = new AppDbContext();  
            // DbInitializer.Initialize(context);                      

            // Danach die MainWindow starten
            var mainWindow = new MainWindow();
            mainWindow.Show();


        }
    }
}