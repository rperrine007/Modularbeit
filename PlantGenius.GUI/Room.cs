using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantGenius.GUI
{
    /// <summary>
    /// A room has a name. If an apartment or a house has different floors, the room also knows in which floor it is. 
    /// 
    /// </summary>
    public class Room
    {
        public Room()
        {}

        /// Constructor
        /// <param name="roomID"></param>
        /// <param name="roomName"></param>
        /// <param name="roomSortNumber"></param>
        public Room(int roomID, string roomName, int roomSortNumber) 
        {
            RoomID = roomID;
            RoomName = roomName;
            FloorOfRoom = 0;
        }


        /// Constructor 2
        /// <param name="roomID"></param>
        /// <param name="roomName"></param>
        /// <param name="roomSortNumber"></param>
        /// <param name="floorOfRoom"></param>
        /// <param name="roomLight"></param>
        public Room(int roomID, string roomName, int roomSortNumber, int floorOfRoom, bool roomLight)
        {
            RoomID = roomID;
            RoomName = roomName;
            RoomSortNumber = roomSortNumber;
            FloorOfRoom = floorOfRoom;
            RoomLight = roomLight;
        }

        //Properties
        public int RoomID { get; init; }
        public string RoomName { get; set; }
        public int RoomSortNumber {  get; set; }
        public int FloorOfRoom { get; set; }
        public bool RoomLight { get; set; }

        //override ToString function
        public override string ToString()
        {
            return $"RaumID: {RoomID}, Raumname: {RoomName}, Stockwerk: {FloorOfRoom}, viel Licht im Raum: {RoomLight}";
        }

    }
}
