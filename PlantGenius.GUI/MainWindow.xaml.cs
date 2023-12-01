using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace PlantGenius.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded; // Register the Loaded event
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var dbConnector = new DatabaseConnector2();
            using (var connection = await dbConnector.GetDatabaseConnectionAsync())
            {
                await ExecuteQueryAsync(connection);
            }
        }

        private async Task ExecuteQueryAsync(MySqlConnection connection)
        {
            string query = "SELECT * FROM Room";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        // Construct the message string
                        string message = $"RoomId: {reader["RoomID"]}, RoomName: {reader["RoomName"]}, RoomSortNumber: {reader["RoomSort"]}, RoomFloor: {reader["RoomFloor"]}, RoomLight: {reader["RoomLight"]}";

                        // Show the message in a MessageBox
                        MessageBox.Show(message);
                    }
                }
            }

            // SQL Querrys Beispiele
            // SELECT * FROM `Room` WHERE `RoomName` = 'Wohnzimmer'     // Gib Raum mit Name "Wohnzimmer" zurück
            // SELECT * FROM `Room` WHERE `RoomID` = '1'                // Gib Raum mit ID 1 zurück
            // SELECT * FROM `Room` ORDER BY `Room`.`RoomFloor` DESC    // Alle Räume aber Absteigend vom obersten zum untersten Stock
        }

    }


}

