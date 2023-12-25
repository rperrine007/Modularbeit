using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PlantGenius.DAL;
using PlantGenius.DAL.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Azure;
using System.DirectoryServices;
using PlantGenius.GUI.Views;

namespace PlantGenius.GUI.ViewModel
{
    /// <summary>
    /// When the RoomView Window is opened, the data of the DB is loaded into an observable collection.
    /// </summary>

    //by inheriting from the ObservableObject the INotifyPeopweryChanged Interface is implemented easily. This interface usually ensures a two was data binding.
    public partial class RoomWindowViewModel : ObservableObject
    {
        //Datavaribles
        private string inputRoomName;
        private int inputRoomSort;
        HashSet<string> existingNames = new HashSet<string>();
        private DataAccessLayer DAL;

        //Properties
        public ObservableCollection<Room> roomList { get; set; }

        public string RoomName { get; set; }

        public string RoomSort { get; set; }

        public string RoomFloor { get; set; }

        public string RoomLight { get; set; }

        // Commands
        // public IAsyncRelayCommand AddRoomCommand => new AsyncRelayCommand(AddRoom);

        //define a Relay Commands which can take two parameters. Tha this works the class MultiParameterValueConverter is necessary.
        public RelayCommand<(object obj, object tag)> ChangeRoomSortNumberCommand { get; }



        //Constructor
        public RoomWindowViewModel()
        {
            //initialize datavariables
            roomList = new ObservableCollection<Room>();
            DAL = new DataAccessLayer();

            this.RoomName = string.Empty;
            this.RoomSort = string.Empty;
            this.RoomFloor = string.Empty;
            this.RoomLight = string.Empty;

            //Special RelayCommands initialization
            ChangeRoomSortNumberCommand = new RelayCommand<(object, object)>((parameters) => ChangeRoomSortNumber(parameters.Item1, parameters.Item2));

            //gert rooms from DB
            getRoomFromDB();
        }

        /// <summary>
        /// Get data through the RoomManager; the data will be reloaded from time to time. The Observable Properties and Collection ensure that the view also get the new data. 
        /// </summary>
        /// 
        //TODO "WHILE(TRUE)" -- For later for GUI and user.GUI
        private async void getRoomFromDB()
        {
                //always clear the roomlist first.
                roomList.Clear();
                existingNames.Clear();
                var rooms = await DAL.GetRooms();
                foreach (var room in rooms)
                {
                    roomList.Add(room);
                    existingNames.Add(room.RoomName);
                }
        }

        private bool CanAddRoom(object obj)
        {
            //check if a Room with the given name already exists.
            if (existingNames.Contains(this.RoomName))
            {
                string title = "Fehler";
                string message = "Raum konnte nicht hinzugefügt werden. \nEs gibt bereits einen Raum mit dem angegeben Namen. Bitte ändere den Namen.";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Adds a new room to the roomList and the DB.
        /// </summary>
        /// <param name="obj"></param>
        [RelayCommand(CanExecute = nameof(CanAddRoom))]
        private async Task AddRoom(object obj)
        {
            Room newRoom = null;

            //checks if a Room is not null
            if (this.RoomName == string.Empty)
            {
                string title = "Fehler";
                string message = "Raum konnte nicht hinzugefügt werden. \nRaumname ist ein Pflichtfeld";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Create a new Room object from the input
            newRoom = new Room()
                {
                    RoomName = this.RoomName, 
                    RoomSort = roomList.Count + 1,
                    RoomFloor = this.RoomFloor == string.Empty ? null : int.Parse(RoomFloor),
                    RoomLight = this.RoomFloor == string.Empty ? null : bool.Parse(this.RoomLight)
                };
            //add new roomname also to HashSet existingNames.
            existingNames.Add(this.RoomName);


            //add Room to DB
            await DAL.AddRoomToDB(newRoom);

            // Add to ObservableCollection
            roomList.Add(newRoom);
        }


        private bool CanDeleteRoom(object obj)
        {
            return true;
        }

        // Delete Room
        [RelayCommand(CanExecute = nameof(CanDeleteRoom))]
        private async Task DeleteRoom(object obj)
        {
            var listBox = obj as ListBox;

            //exception handling in case no room is chosen.
            try
            {
                var selectedRoom = listBox.SelectedItem as Room;
                await DAL.DeleteRoomFromDB(selectedRoom);
                // Remove the specified room from the ObservableCollection
                roomList.Remove(selectedRoom);

            }
            catch (ArgumentNullException e)
            {
                string title = "Fehler";
                string message = "Bitte wählen Sie einen Raum aus!";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //update view again. Else the SortNumber would not be correct.
            getRoomFromDB();
        }


        /// <summary>
        /// The SortID will be decreased(-1) or increased (1) and the direct neighbour will be swapped with the choosen room. 
        /// Why asynchronous: This ensures t hat the application remains responsive and can handle
        /// </summary>
        /// <param name="obj"></param>
        private async Task ChangeRoomSortNumber(object obj, object tag)
        {

            ListBox? listBox = obj as ListBox;
            int currentIndex = listBox.SelectedIndex;
            var direction = int.Parse(tag.ToString());

            //Kontrolliere ob Raum ausgewählt wurde.
            if (currentIndex == -1)
            {
                string title = "Fehler";
                string message = "Bitte wählen Sie einen Raum aus!";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int newIndex = currentIndex + direction;
            // Check index bandwith for up and down movement
            if (newIndex < 0 || newIndex >= roomList.Count)
            {
                string title = "Fehler";
                string message = "Bewegung in diese Richtung nicht möglich!";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //initialize room varibales
            Room currentRoom = roomList[currentIndex];
            Room neighbourRoom = roomList[newIndex];

            // Swap sort numbers
            int? tempSortNumber = currentRoom.RoomSort;
            currentRoom.RoomSort = neighbourRoom.RoomSort;
            neighbourRoom.RoomSort = tempSortNumber;

            // Swap order
            roomList[currentIndex] = neighbourRoom;
            roomList[newIndex] = currentRoom;


            // Update DB for both rooms
            await DAL.UpdateRoomSortNumber(currentRoom.RoomID, currentRoom.RoomSort ?? roomList.Count - 1);
            await DAL.UpdateRoomSortNumber(neighbourRoom.RoomID, neighbourRoom.RoomSort ?? roomList.Count);

            // Keep focus on moved object
            listBox.SelectedIndex = newIndex;
        }


        private bool CanSaveChanges(object obj)
        {
            return true;
        }


        /// <summary>
        /// This Method updates a room to the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        // Delete Room
        [RelayCommand(CanExecute = nameof(CanSaveChanges))]
        private async void SaveChanges(object obj)
        {

            ListBox? listBox = null;
            Room? selectedRoom = null;

            //Exception handling in case: no item of the listBox is chosen by the user, object is not
            try
            {
                listBox = obj as ListBox;
                selectedRoom = listBox.SelectedItem as Room;

            }
            catch (NullReferenceException ex)
            {
                string title = "Fehler";
                string message = "Bitte wählen Sie einen Raum aus!";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //TODO does not work properly. 
            // Check if a Room with the given name already exists and is not the current room.
            if (existingNames.Contains(selectedRoom?.RoomName) && !selectedRoom.RoomName.Equals(this.RoomName))
            {
                string title = "Fehler";
                string message = "Raum konnte nicht hinzugefügt werden. \n Es gibt bereits einen Raum mit dem angegeben Namen. Bitte ändere den Namen.";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);                
            }
            else
            {
                // Call the method to update the room in the database
                await DAL.UpdateRoomToDB(selectedRoom);

            }

            getRoomFromDB();
        }

        private bool CanShowMainWindow(object obj)
        {
            //We always want to execute the command.
            return true;
        }

        [RelayCommand(CanExecute = nameof(CanShowMainWindow))]
        private void ShowMainWindow(object obj)
        {
            MainWindow mainViewWin = new MainWindow();
            mainViewWin.Show();

            //initialize variabel with defined command parameter and cast it as type Window
            var roomView = obj as Window;

            try
            {
                // Save the position of the window to keep size and position
                mainViewWin.Left = roomView.Left;
                mainViewWin.Top = roomView.Top;
                mainViewWin.Width = roomView.Width;
                mainViewWin.Height = roomView.Height;

                // Open the new window and close old one
                mainViewWin.Show();
                roomView.Close();
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}


    




