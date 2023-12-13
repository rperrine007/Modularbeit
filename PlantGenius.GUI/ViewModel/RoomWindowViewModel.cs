using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using PlantGenius.DAL;
using PlantGenius.DAL.Models;

namespace PlantGenius.GUI.ViewModel
{
    /// <summary>
    /// When the RoomView Window is opened, the data of the DB is loaded into an observable collection.
    /// </summary>
    public class RoomWindowViewModel
    {
        public ObservableCollection<Room> roomList {  get; set; }


        public RoomWindowViewModel()
        {
            roomList = new ObservableCollection<Room>();
            getRoomFromRoomManager();
        }



        /// <summary>
        /// Get data through the RoomManager
        /// </summary>
        private async void getRoomFromRoomManager()
        {
            var rooms = await DataAccessLayer.GetRooms();
            roomList.Clear();
            foreach (var room in rooms)
            {
                roomList.Add(room);
            }
        }


    }
}
