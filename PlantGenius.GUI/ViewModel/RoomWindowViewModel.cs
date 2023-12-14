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

namespace PlantGenius.GUI.ViewModel
{
    /// <summary>
    /// When the RoomView Window is opened, the data of the DB is loaded into an observable collection.
    /// </summary>
    
    //by inheriting from the ObservableObject the INotifyPeopweryChanged Interface is implemented easily. This interface usually ensures a two was data binding.
    public partial class RoomWindowViewModel : ObservableObject
    {
        //Datavariables
        public ObservableCollection<Room> roomList {  get; set; }

        public string RoomName { get; set; }

        public int RoomSort { get; set; }

        public string RoomFloor { get; set; }

        public bool RoomLight { get; set; }

        // Commands
        public IAsyncRelayCommand AddRoomCommand => new AsyncRelayCommand(AddRoom);

        public RoomWindowViewModel()
        {
            //initialize datavariables
            roomList = new ObservableCollection<Room>();

            this.RoomName = string.Empty;
            this.RoomSort = 999;

            //TODO ändere zu string und mache ein parse. 
            this.RoomFloor = string.Empty;
            this.RoomLight = true;

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

        /// <summary>
        /// Adds a new room to the roomList and the DB.
        /// </summary>
        /// <param name="obj"></param>
        private async Task AddRoom()
        {
            // Create a new Room object from the input
            Room newRoom = new Room()
            {
                RoomName = this.RoomName,
                RoomSort = roomList.Count + 1,
                RoomFloor = int.Parse(this.RoomFloor),
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
        [RelayCommand(CanExecute = nameof(CanChangeRoomSortNumber))]
        private async Task ChangeRoomSortNumber(object obj)
        {
            var listBox = obj as ListBox;
            //Gerüst für zwei Paramter
            /*
            var parameters = obj as object[];
            if (parameters == null || parameters.Length != 2)
            {
                // Handle invalid parameters
                return;
            }

            var listBox = parameters[0] as ListBox;
            var direction = parameters[1] as int?;*/

            // Validation if room choosen
            if (listBox.SelectedItem == null)
            {
                string title = "Fehler";
                string message = "Bitte wählen Sie einen Raum aus!";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int currentIndex = listBox.SelectedIndex;

            // TODO: Ersetze mit direction wenn zwei Parameter übergeben werden können.
            int newIndex = currentIndex - 1;

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
            int? tempSortNumber = currentRoom.RoomSort;
            currentRoom.RoomSort = neighbourRoom.RoomSort;
            neighbourRoom.RoomSort = tempSortNumber;

            // Swap order
            roomList[currentIndex] = neighbourRoom;
            roomList[newIndex] = currentRoom;

            // Update DB for both rooms
            await DataAccessLayer.UpdateRoomSortNumber(currentRoom.RoomID, currentRoom.RoomSort ?? roomList.Count + 1);
            await DataAccessLayer.UpdateRoomSortNumber(neighbourRoom.RoomID, neighbourRoom.RoomSort ?? roomList.Count);

            // Keep focus on moved object
            listBox.SelectedIndex = newIndex;
        }


        private bool CanChangeRoomSortNumber_Down(object obj)
        {
            return true;
        }

        /// <summary>
        /// The SortID will be decreased(-1) or increased (1) and the direct neighbour will be swapped with the choosen room. 
        /// Why asynchronous: This ensures t hat the application remains responsive and can handle
        /// </summary>
        /// <param name="obj"></param>
        [RelayCommand(CanExecute = nameof(CanChangeRoomSortNumber_Down))]
        private async Task ChangeRoomSortNumber_Down(object obj)
        {
            var listBox = obj as ListBox;
            //Gerüst für zwei Paramter
            /*
            var parameters = obj as object[];
            if (parameters == null || parameters.Length != 2)
            {
                // Handle invalid parameters
                return;
            }

            var listBox = parameters[0] as ListBox;
            var direction = parameters[1] as int?;*/

            // Validation if room choosen
            if (listBox.SelectedItem == null)
            {
                string title = "Fehler";
                string message = "Bitte wählen Sie einen Raum aus!";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int currentIndex = listBox.SelectedIndex;

            // TODO: Ersetze mit direction wenn zwei Parameter übergeben werden können.
            int newIndex = currentIndex + 1;

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
            int? tempSortNumber = currentRoom.RoomSort;
            currentRoom.RoomSort = neighbourRoom.RoomSort;
            neighbourRoom.RoomSort = tempSortNumber;

            // Swap order
            roomList[currentIndex] = neighbourRoom;
            roomList[newIndex] = currentRoom;

            // Update DB for both rooms
            // Update DB for both rooms
            await DataAccessLayer.UpdateRoomSortNumber(currentRoom.RoomID, currentRoom.RoomSort ?? roomList.Count - 1);
            await DataAccessLayer.UpdateRoomSortNumber(neighbourRoom.RoomID, neighbourRoom.RoomSort ?? roomList.Count);

            // Keep focus on moved object
            listBox.SelectedIndex = newIndex;
        }

        //TODO vervollständige diese Funtion. Hierfür müssen aber zwei Parameter oder mehr einem Command übergeben werden können.
        /*
        private bool CanSaveChanges(object obj)
        {
            return true;
        }

        /// <summary>
        /// This Method updates a room to the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SaveChanges(object obj)
        {
            var listBox = obj as ListBox;
            if (listBox.SelectedItem is Room selectedRoom)
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
        }*/
    }
}
