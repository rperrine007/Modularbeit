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
using PlantGenius.GUI.Commands;

namespace PlantGenius.GUI.ViewModel
{
    /// <summary>
    /// When the RoomView Window is opened, the data of the DB is loaded into an observable collection.
    /// </summary>
    public class RoomWindowViewModel
    {
        //Datavariables
        public ObservableCollection<Room> roomList {  get; set; }
        public string RoomName { get; set; }
        public int RoomSort { get; set; }
        public int RoomFloor { get; set; }
        public bool RoomLight { get; set; }

        //Commands
        public ICommand AddRoomCommand {  get; set; }
        public ICommand DeleteRoomCommand { get; set; }
        public ICommand ChangeRoomSortNumberCommand { get; set; }




        public RoomWindowViewModel()
        {
            //initialize datavariables
            roomList = new ObservableCollection<Room>();
            this.RoomName = "Benutzereingabe";
            this.RoomSort = 0;
            this.RoomFloor = 0;
            this.RoomLight = false;


            //Initialize commands
            AddRoomCommand = new RelayCommand(AddRoom, CanAddRoom);
            DeleteRoomCommand = new RelayCommand(DeleteRoom, CanDeleteRoom);
            ChangeRoomSortNumberCommand = new RelayCommand(ChangeRoomSortNumber, CanChangeRoomSortNumber);

            //gert rooms from DB
            getRoomFromDB();
        }

        /// <summary>
        /// Get data through the RoomManager
        /// </summary>
        private async void getRoomFromDB()
        {
            var rooms = await DataAccessLayer.GetRooms();
            roomList.Clear();
            foreach (var room in rooms)
            {
                roomList.Add(room);
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
        private async void AddRoom(object obj)
        {
            // Create a new Room object from the input
            Room newRoom = new Room()
            {
                RoomName = this.RoomName,
                RoomSort = roomList.Count + 1,
                RoomFloor = this.RoomFloor,
                RoomLight = this.RoomLight
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

            private async void DeleteRoom(object obj)
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
            catch(ArgumentNullException e)
            {
                MessageBox.Show("Bitte wählen Sie einen Raum aus.");
                Console.WriteLine(e.Message);
            }

        }

        private bool CanChangeRoomSortNumber(object obj)
        {
            return true;
        }

        /// <summary>
        /// The SortID will be decreased(-1) or increased (1) and the direct neighbour will be swapped with the choosen room. 
        /// Why asynchronous: This ensures t hat the application remains responsive and can handle
        /// </summary>
        /// <param name="obj"></param>
        private async void ChangeRoomSortNumber(object obj)
        {
            var listBox = obj as ListBox;
            // Validation if room choosen
            if (listBox.SelectedItem == null)
            {
                string title = "Fehler";
                string message = "Bitte wählen Sie einen Raum aus!";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int currentIndex = listBox.SelectedIndex;
            int newIndex = currentIndex -1;

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
            listBox.SelectedIndex = newIndex;
        }


    }
}
