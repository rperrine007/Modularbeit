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
        //  public RelayCommand<(object obj, object obj2)> SaveChangesCommand { get; }



        //Constructor
        public RoomWindowViewModel()
        {
            //initialize datavariables
            roomList = new ObservableCollection<Room>();

            this.RoomName = string.Empty;
            this.RoomSort = string.Empty;
            this.RoomFloor = string.Empty;
            this.RoomLight = string.Empty;

            //Special RelayCommands initialization
            ChangeRoomSortNumberCommand = new RelayCommand<(object, object)>((parameters) => ChangeRoomSortNumber(parameters.Item1, parameters.Item2));
            //  SaveChangesCommand = new RelayCommand<(object, object)>((parameters) => SaveChanges(parameters.Item1);

            //gert rooms from DB
            getRoomFromDB();
        }

        /// <summary>
        /// Get data through the RoomManager; the data will be reloaded from time to time. The Observable Properties and Collection ensure that the view also get the new data. 
        /// </summary>
        /// 

        //TODO "WHILE(TRUE)"
        private async void getRoomFromDB()
        {
            while (true)
            {
                var rooms = await DataAccessLayer.GetRooms();
                roomList.Clear();
                foreach (var room in rooms)
                {
                    roomList.Add(room);
                }
                await Task.Delay(100000);
            }
        }

        private bool CanAddRoom(object obj)
        {
            return true;
        }
        /// <summary>
        /// Adds a new room to the roomList and the DB.
        /// </summary>
        /// <param name="obj"></param>


        // Delete Room
        [RelayCommand(CanExecute = nameof(CanAddRoom))]
        private async Task AddRoom(object obj)
        {
            // Create a new Room object from the input
            Room newRoom = new Room()
            {
                RoomName = this.RoomName,
                RoomSort = roomList.Count + 1,
                RoomFloor = int.Parse(this.RoomFloor),
                RoomLight = bool.Parse(this.RoomLight)
            };

            //add Room to DB
            await DataAccessLayer.AddRoomToDB(newRoom);

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
                await DataAccessLayer.DeleteRoomFromDB(selectedRoom);
                // Remove the specified room from the ObservableCollection
                roomList.Remove(selectedRoom);

            }
            catch (ArgumentNullException e)
            {
                MessageBox.Show("Bitte wählen Sie einen Raum aus.");
                Console.WriteLine(e.Message);
            }
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
            await DataAccessLayer.UpdateRoomSortNumber(currentRoom.RoomID, currentRoom.RoomSort ?? roomList.Count - 1);
            await DataAccessLayer.UpdateRoomSortNumber(neighbourRoom.RoomID, neighbourRoom.RoomSort ?? roomList.Count);

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
            Room selectedRoom = null;

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

            //Create Rooom
            Room userInputBoxRoom = new Room()
            {
                RoomName = selectedRoom.RoomName,
                RoomSort = selectedRoom.RoomSort,
                RoomFloor = int.Parse(this.RoomFloor),
                RoomLight = bool.Parse(this.RoomLight)
            };
            // Call the method to update the room in the database
            await DataAccessLayer.UpdateRoomToDB(userInputBoxRoom);
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


    




