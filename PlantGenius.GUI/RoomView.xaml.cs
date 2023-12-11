using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using PlantGenius.DAL;

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
        private DatabaseConnector dbConnector;

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

        /// <summary>
        /// Load the initial view including importing the data of the db.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RoomView_Loaded(object sender, RoutedEventArgs e)
        {
                //import rooms from db and save into roomList
                await DataAccessLayer.GetRooms(dbConnector, roomList);
        }

        /// <summary>
        /// go back to the mainpage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoBackToHome(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            UIHelper.SwitchWindowKeepSizePosition(this, mainWindow);
        }

        /// <summary>
        /// The SortID will be decreased(-1) or increased (1) and the direct neighbour will be swapped with the choosen room. 
        /// Why asynchronous: This ensures that the application remains responsive and can handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="direction"></param>
        private async Task ChangeRoomSortNumber(object sender, RoutedEventArgs e, int direction)
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
                await DataAccessLayer.UpdateDBChangeRoomSortNumber(connection, currentRoom.RoomID, currentRoom.RoomSortNumber);
                await DataAccessLayer.UpdateDBChangeRoomSortNumber(connection, neighbourRoom.RoomID, neighbourRoom.RoomSortNumber);
            }

            // Keep focus on moved object
            ListBox_RoomList.SelectedIndex = newIndex;
        }      
     
        /// <summary>
        /// Gets the tag from the button and its value -> adds it to the int direction. Will be handled from ChangeRoomSortNumber 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DirectionOfRoomSortNumberChanged(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                int direction = Convert.ToInt32(button.Tag);

                // Call ChangeRoomSortNumber method with the direction parameter
                await ChangeRoomSortNumber(sender, e, direction);
                await DataAccessLayer.OnRoomDeleteNewSort(dbConnector, roomList);
            }
        }

        /// <summary>
        /// This Method deletes a room entry from the database. Afterwards it will update the sorting numbers to avoid gaps
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Delete(object sender, RoutedEventArgs e)
        {
            //control if a room is chosen
            if (ListBox_RoomList.SelectedItem == null)
            {
                MessageBox.Show("Bitte wählen Sie einen Raum aus.");
                return;
            }

            Room currentRoom = (Room)ListBox_RoomList.SelectedItem;

            // Remove the specified room from the ObservableCollection
            roomList.Remove(currentRoom);

            await DataAccessLayer.DeleteRoomFromDB(dbConnector, roomList, currentRoom);
        }

        /// <summary>
        /// A new room is added to the list and the db.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddNewRoom_Click(object sender, RoutedEventArgs e)
        {
            
            // Retriving the data from the dropdown and handle it
            bool roomLight = false;
            var selectedItem = comboBoxRoomLight.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                // Use roomLight as Boolean value
                roomLight = bool.Parse(selectedItem.Tag.ToString());
            }

            // Create a new Room object from the input
            Room newRoom = new Room()
            {
                RoomName = inputNewRoomName.Text,
                RoomSortNumber = roomList.Count + 1,
                FloorOfRoom = int.Parse(inputNewRoomFloor.Text),
                RoomLight = roomLight
            };
            // Add to ObservableCollection
            roomList.Add(newRoom);

            //add Room to DB
            await DataAccessLayer.AddRoomToDB(dbConnector, newRoom);
        }


        /// <summary>
        /// Prevents to add non int values to a textfield.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //TODO unit test and Exception handling for this method 
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender != null)
            {
                TextBox textBox = sender as TextBox;
                // Allow "-" only if it's the first character, allow digits
                if (e.Text == "-" && textBox.Text.Length == 0 && !textBox.Text.Contains("-"))
                {
                    // Allow input
                    e.Handled = false; 
                }
                else
                {
                    // Allow digits only
                    foreach (char c in e.Text)
                    {
                        if (!char.IsDigit(c))
                        {
                            // Block input
                            e.Handled = true; 
                            break;
                        }
                    }
                }
            }
        }

    }
}
