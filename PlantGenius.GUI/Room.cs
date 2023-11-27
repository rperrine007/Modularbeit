using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//TODO: Should the room have a list with plants?

namespace PlantGenius.GUI
{
    /// <summary>
    /// A room has a name. If an apartment or a house has different floors, the room also knows in which floor it is. 
    /// In a room a set of plants can be placed. 

    /// 
    /// </summary>
    public class Room
    {
        /// Constructor
        /// <param name="roomName"></param>
        public Room(string roomName) 
        { 
            RoomName = roomName;
            FloorOfRoom = 0;
        }

        /// Constructor 2
        /// <param name="roomName"></param>
        /// <param name="floorofRoom"></param>
        public Room(string roomName, int floorofRoom)
        {
            RoomName = roomName;
            FloorOfRoom = floorofRoom;
        }

        public string RoomName { get; set; }
        public int FloorOfRoom { get; set; }
    }
}
