using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantGenius.DAL.Models
{
    public class RoomManager
    {
        public static ObservableCollection <Room> DatabaseRoomList { get; set; }

        public RoomManager()
        {
            DatabaseRoomList = new ObservableCollection<Room>();
        }

        /// <summary>
        /// Load the initial view including importing the data of the db.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async static Task<ObservableCollection<Room>> getRooms()
        { 
            var rooms = await DataAccessLayer.GetRooms();
            DatabaseRoomList.Clear();
            foreach (var room in rooms)
            {
                DatabaseRoomList.Add(room);
            }
            return DatabaseRoomList;
        }

                /// <summary>
        /// A new room is added to the list and the db.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async static void addRooms(Room room)
        {
            //add Room to DB
            await DataAccessLayer.AddRoomToDB(room);

            // Add to ObservableCollection
            DatabaseRoomList.Add(room);
        }

    }
}
