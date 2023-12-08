using Microsoft.Extensions.Logging;
using System;
using MySql.Data.MySqlClient;

namespace PlantGenius_User.GUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            var app = builder.Build();

            // Attempt to connect to the database and print the status to the console
            //var dbConnector = new DatabaseConnector();
            //dbConnector.ConnectToDatabase();

            return app;
        }
    }
}