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
            // Create an instance of the DatabaseConnector2 class.
            var dbConnector = new DatabaseConnector2();

            // Use the 'GetDatabaseConnectionAsync' method to asynchronously obtain a database connection.
            // The 'await' keyword is used to await the completion of the asynchronous operation.
            using (var connection = await dbConnector.GetDatabaseConnectionAsync())
            {
                // The obtained database connection is now used to execute an asynchronous database query.
                // The 'await' keyword ensures that the 'ExecuteQueryAsync' method is awaited,
                await GetRooms(connection);
            }
        }

        /// <summary>
        /// In this asynchronous task a query to get the room data is made to the DB.
        /// Why asynchronous: This ensures that the application remains responsive and can handle
        /// other tasks while waiting for the database to return results.
        ///  </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private async Task GetRooms(MySqlConnection connection)
        {
            //string to get all columns (*) from the table Room on the DB.
            string query = "SELECT * FROM Room";

            // the defined query is made on the defined connection (DB)
            using (var command = new MySqlCommand(query, connection))
            {
                //the data is asynchroned read from the DB. 
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

            // Neuer Datensatz einfügen
            // INSERT INTO `Room` (`RoomID`, `RoomName`, `RoomSort`, `RoomFloor`, `RoomLight`) VALUES (NULL, 'Testraum', '0', '2', '0');

            // Datensatz anpassen // vorgängig sinnvollerweise ein SELECT ID, Übergabe der Inhalte in Formular und dann zurück mit Update mit geänderten Inhalten
            // UPDATE `Room` SET `RoomFloor` = '1' WHERE `Room`.`RoomID` = 4;
        }

    }


}

