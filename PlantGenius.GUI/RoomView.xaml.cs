using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        // Go back to main window MainWindow.xaml
        private void GoBackToHome(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            UIHelper.SwitchWindowKeepSizePosition(this, mainWindow);
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

        //Open new window to add new room
        //TODO create RoomAddingView based on RoomView (second Row, all Coloumns. Here you have to create a new .xaml File. Do not forget to name it directly correct.
        //TODO implement the function so that the RoomAddingView is opened
        private void OpenRoomAddingView(object sender, RoutedEventArgs e)
        {

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

        private async 
        /// <summary>
        /// The SortID will be decreased(-1) or increased (1) and the direct neighbour will be swapped with the choosen room. 
        /// Why asynchronous: This ensures that the application remains responsive and can handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="direction"></param>
        Task
        ChangeRoomSortNumber(object sender, RoutedEventArgs e, int direction)
        {
            // Validation if room choosen
            if (ListBox_RoomList.SelectedItem == null)
            {
                MessageBox.Show("Bitte wählen Sie einen Raum aus.");
                return;
            }

            int currentIndex = ListBox_RoomList.SelectedIndex;
            int newIndex = currentIndex + direction;

            // Check index bandwith for up and down movement
            if (newIndex < 0 || newIndex >= roomList.Count)
            {
                MessageBox.Show("Bewegung in diese Richtung nicht möglich.");
                return;
            }

            Room currentRoom = roomList[currentIndex];
            Room neighbourRoom = roomList[newIndex];

            // Swap sort numbers
            int tempSortNumber = currentRoom.RoomSortNumber;
            currentRoom.RoomSortNumber = neighbourRoom.RoomSortNumber;
            neighbourRoom.RoomSortNumber = tempSortNumber;

            // Swap order
            roomList[currentIndex] = neighbourRoom;
            roomList[newIndex] = currentRoom;

            // Update DB for both rooms
            using (var connection = await dbConnector.GetDatabaseConnectionAsync())
            {
                await ChangeSortRoomNumber(connection, currentRoom.RoomID, currentRoom.RoomSortNumber);
                await ChangeSortRoomNumber(connection, neighbourRoom.RoomID, neighbourRoom.RoomSortNumber);
            }
        }

        /// <summary>
        /// Gets the Tag from the Button and its value -> adds it to the int direction. Will be handlet from ChangeRoomSortNumber 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnRoomSortNumberChanged(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                //TODO Add some error handling as this could go really wrong o.O or if there is a possiblity to make sure the tag is handled as int would be much better

                int direction = Convert.ToInt32(button.Tag);

                // Call ChangeRoomSortNumber method with the direction parameter
                await ChangeRoomSortNumber(sender, e, direction);
            }
        }

        /// <summary>
        /// The index and RoomSortNumber of the by the user chosen room will be decreased and hence the room one index lower accordingly changed. 
        /// Why asynchronous: This ensures that the application remains responsive and can handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        //private async void changeroomsortnumberup(object sender, routedeventargs e)
        //{
        //    //make sure the user chose a room and its not the first room.
        //    if (listbox_roomlist.selecteditem != null && listbox_roomlist.selectedindex > 0)
        //    {
        //        //get index of selected room and corresponding object
        //        int currentindex = listbox_roomlist.selectedindex;
        //        room selectedroom = roomlist[currentindex];
        //        //save the element with one index lower
        //        room previousroom = roomlist[currentindex - 1];


        //        //change sortnumber
        //        selectedroom.roomsortnumber--;
        //        previousroom.roomsortnumber++;

        //        //swap order
        //        roomlist[currentindex - 1] = selectedroom;
        //        roomlist[currentindex] = previousroom;

        //        //update db
        //        // use the 'getdatabaseconnectionasync' method to asynchronously obtain a database connection.
        //        // the 'await' keyword is used to await the completion of the asynchronous operation.
        //        using (var connection = await dbconnector.getdatabaseconnectionasync())
        //        {
        //            // the obtained database connection is now used to execute an asynchronous database query.
        //            // the 'await' keyword ensures that the 'executequeryasync' method is awaited,
        //            await changesortroomnumber(connection, selectedroom.roomid, selectedroom.roomsortnumber);
        //            await changesortroomnumber(connection, previousroom.roomid, previousroom.roomsortnumber);
        //        }
        //    }
        //    else if (listbox_roomlist.selecteditem == null)
        //    {
        //        messagebox.show("bitte wählen sie den raum an, welchen sie in der darstellung nach oben verschieben möchten.");
        //    }
        //    else
        //    {
        //        messagebox.show("der gewählt raum ist bereits der erste in der liste und kann daher nicht weiter nach oben verschoben werden.");
        //    }
        //}

        // TODO implement method similar to ChageRoomSortNumberUp

        private void Delete(object sender, RoutedEventArgs e)
        {

        }
    }
}
