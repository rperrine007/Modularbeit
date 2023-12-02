using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;

namespace PlantGenius.GUI
{
    /// <summary>
    /// Interaction logic for RoomView.xaml
    /// INotifyPropertyChanged interface need to be added to make a two way interaction with the UI interface (data binding context).
    /// </summary>
    public partial class RoomView : Window
    { 

        // Generate a Collection with rooms
        private ObservableCollection<Room> roomList;
        // Create an instance of the DatabaseConnector class.
        DatabaseConnector dbConnector;



        public RoomView()
        {
            InitializeComponent();

            //create objects
            roomList = new ObservableCollection<Room>();
            dbConnector = new DatabaseConnector();

            //Set Datacontext for binding in WPF
            ListBox_RoomList.DataContext = roomList;
            StackPanel_chosenRoom.DataContext = roomList;

            //Load window and process function. The function includes the data import of the DB
            Loaded += RoomView_Loaded;
        }

        private async void RoomView_Loaded(object sender, RoutedEventArgs e)
        {
            // Use the 'GetDatabaseConnectionAsync' method to asynchronously obtain a database connection.
            // The 'await' keyword is used to await the completion of the asynchronous operation.
            using (var connection = await dbConnector.GetDatabaseConnectionAsync())
            {
                // The obtained database connection is now used to execute an asynchronous database query.
                // The 'await' keyword ensures that the 'ExecuteQueryAsync' method is awaited,
                await GetRooms(connection);
            }
        }

        // Go back to main window
        private void GoBackToHome(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
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
            // Sort by set RoomSort.
            string query = "SELECT * FROM Room ORDER BY RoomSort ASC";

            // the defined query is made on the defined connection (DB)
            using (var command = new MySqlCommand(query, connection))
            {
                //the data is asynchroned read from the DB. 
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        //Add Rooms of DB to roomList
                        roomList.Add(new Room()
                        {
                            RoomID = (int)reader["RoomID"],
                            RoomName = (string)reader["RoomName"],
                            RoomSortNumber = (int)reader["RoomSort"],
                            FloorOfRoom = (int)reader["RoomFloor"],
                            RoomLight = (bool)reader["RoomLight"]
                        });
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

            // INSERT INTO `Plant` (`PlantID`, `PlantName`, `PlantNameScientific`, `PlantRoom`, `PlantSort`, `PlantWaterRequirement`, `PlantWaterLastTime`) VALUES (NULL, 'Orchideen', 'Orchidaceae', '1', '0', '14', '2023-12-01');
        }

        /// <summary>
        /// In this asynchronous task a query to get the room data is made to the DB.
        /// Why asynchronous: This ensures that the application remains responsive and can handle
        /// other tasks while waiting for the database to return results.
        ///  </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private async Task ChangeSortRoomNumber(MySqlConnection connection, int roomIDSelected, int roomSortChanged)
        {
            // query to update the roomSortNumber of a specific room
            string query = $"UPDATE `Room` SET `Room`.`RoomSort` = '{roomSortChanged}' WHERE `Room`.`RoomID` = {roomIDSelected}";

            // Using MySqlCommand to execute the query on the specified connection
           using (var command = new MySqlCommand(query, connection))
           {
                // Executing the query asynchronously
                await command.ExecuteNonQueryAsync();
           }
                    
        }

        //Open new window to add new room
        //TODO create RoomAddingView based on RoomView (second Row, all Coloumns. Here you have to create a new .xaml File. Do not forget to name it directly correct.
        //TODO implement the function so that the RoomAddingView is opened
        private void OpenRoomAddingView(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// The index and RoomSortNumber of the by the user chosen room will be decreased and hence the room one index lower accordingly changed. 
        /// Why asynchronous: This ensures that the application remains responsive and can handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ChangeRoomSortNumberUp(object sender, RoutedEventArgs e)
        {
            //make sure the user chose a Room and its not the first room.
            if (ListBox_RoomList.SelectedItem != null && ListBox_RoomList.SelectedIndex > 0)
            {
                //Get index of selected room and corresponding object
                int currentIndex = ListBox_RoomList.SelectedIndex;
                Room selectedRoom = roomList[currentIndex];
                //save the element with one Index lower
                Room previousRoom = roomList[currentIndex - 1];


                //Change sortNumber
                selectedRoom.RoomSortNumber--;
                previousRoom.RoomSortNumber++;

                //swap order
                roomList[currentIndex - 1] = selectedRoom;
                roomList[currentIndex] = previousRoom;

                //update DB
                // Use the 'GetDatabaseConnectionAsync' method to asynchronously obtain a database connection.
                // The 'await' keyword is used to await the completion of the asynchronous operation.
                using (var connection = await dbConnector.GetDatabaseConnectionAsync())
                {
                    // The obtained database connection is now used to execute an asynchronous database query.
                    // The 'await' keyword ensures that the 'ExecuteQueryAsync' method is awaited,
                    await ChangeSortRoomNumber(connection, selectedRoom.RoomID, selectedRoom.RoomSortNumber);
                }                
            }
            else if(ListBox_RoomList.SelectedItem == null)
            {
                MessageBox.Show("Bitte wählen Sie den Raum an, welchen sie in der Darstellung nach oben verschieben möchten.");
            }
            else
            {
                MessageBox.Show("Der gewählt Raum ist bereits der Erste in der Liste und kann daher nicht weiter nach oben verschoben werden.");
            }
        }

        // TODO implement method similar to ChageRoomSortNumberUp
        private void ChangeRoomSortNumberDown (object sender, RoutedEventArgs e)
        {

        }

        private void Delete(object sender, RoutedEventArgs e)
        {

        }
    }
}
