using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MySqlX.XDevAPI.Common;
using PlantGenius.DAL;
using PlantGenius.DAL.Models;
using PlantGenius.GUI.ViewModel;
using PlantGenius.GUI.Views;

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

        public RoomView()
        {
            InitializeComponent();
            RoomWindowViewModel roomViewModel = new RoomWindowViewModel();

            //Set Datacontext for binding in WPF
            this.DataContext = roomViewModel;

            //Set sub-datacontext
            roomList = roomViewModel.roomList;
            ListBox_RoomList.DataContext = roomList;
            StackPanel_chosenRoom.DataContext = roomList;

            //Load window and process function. The function includes the data import of the DB
            //Loaded += RoomView_Loaded;
        }


        /// <summary>
        /// Test the connection to the database
        /// </summary>
        //TODO Test if Method works even with wrong credentials
        private async void TestConnection()
        {
            try
            {
                var result = await DataAccessLayer.TestDatabaseConnectionAsync();

                if (result.connectionStatus)
                {
                    //For debugging only, shows if connection is ok
                    //string title = "SQL Server";
                    //string message = "Verbindung OK!";
                    //MessageBox.Show(message, title);
                }
                else
                {
                    await Console.Out.WriteLineAsync(result.errorMessage);
                    MessageBox.Show($"Verbindungsfehler: {result.errorMessage}");
                }
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                MessageBox.Show($"Unerwarteter Fehler: {e.Message}");
            }
        }


        /// <summary>
        /// go back to the mainpage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoBackToHome(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            //UIHelper.SwitchWindowKeepSizePosition(this, mainWindow);
        }

        
        /// <summary>
        /// The SortID will be decreased(-1) or increased (1) and the direct neighbour will be swapped with the choosen room. 
        /// Why asynchronous: This ensures t hat the application remains responsive and can handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="direction"></param>
        private async Task ChangeRoomSortNumber(object sender, RoutedEventArgs e, int direction)
        {
            // Validation if room choosen
            if (ListBox_RoomList.SelectedItem == null)
            {
                string title = "Fehler";
                string message = "Bitte wählen Sie einen Raum aus!";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int currentIndex = ListBox_RoomList.SelectedIndex;
            int newIndex = currentIndex + direction;

            // Check index bandwith for up and down movement
            if (newIndex < 0 || newIndex >= roomList.Count)
            {
                string title = "Fehler";
                string message = "Bewegung in diese Richtung nicht möglich!";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Room currentRoom = roomList[currentIndex];
            Room neighbourRoom = roomList[newIndex];

            // Swap sort numbers
            int tempSortNumber = currentRoom.RoomSort;
            currentRoom.RoomSort = neighbourRoom.RoomSort;
            neighbourRoom.RoomSort = tempSortNumber;

            // Swap order
            roomList[currentIndex] = neighbourRoom;
            roomList[newIndex] = currentRoom;

            // Update DB for both rooms
            await DataAccessLayer.UpdateRoomSortNumber(currentRoom.RoomID, currentRoom.RoomSort);
            await DataAccessLayer.UpdateRoomSortNumber(neighbourRoom.RoomID, neighbourRoom.RoomSort);

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
            }
        }

        /// <summary>
        /// This Method updates a room to the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox_RoomList.SelectedItem is Room selectedRoom)
            {
                bool roomLight = false;
                var selectedItem = comboBoxRoomLight_edit.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    // Use roomLight as Boolean value
                    roomLight = bool.Parse(selectedItem.Tag.ToString());
                }
                // Update the selected room with changes made in the editable fields
                selectedRoom.RoomName = inputRoomName.Text;
                selectedRoom.RoomFloor = int.Parse(inputRoomFloor.Text);
                selectedRoom.RoomLight = roomLight;

                // Call the method to update the room in the database
                await DataAccessLayer.UpdateRoomToDB(selectedRoom);


                string title = "Update OK";
                string message = "Update wurde durchgeführt!";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string title = "Update Fehler";
                string message = "Bitte zuerst einen Raum auswählen!";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
