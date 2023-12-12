using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantGenius.DAL.Model
{
    /// <summary>
    /// A room has a name. If an apartment or a house has different floors, the room also knows in which floor it is. 
    /// </summary>
    public class Room
    {
        // EF needs this constructor even though it is never called. Else the "No suitable constructor found exception" is thrown.
        public Room()
        { }

        /// Constructor
        /// <param name="roomName"></param>
        /// <param name="floorOfRoom"></param>
        /// <param name="roomLight"></param>
        public Room(string roomName, int floorOfRoom, bool roomLight)
        {
            RoomName = roomName;
            RoomFloor = floorOfRoom;
            RoomLight = roomLight;
            RoomSort = 0;
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
            RoomSort = roomSortNumber;
            RoomFloor = floorOfRoom;
            RoomLight = roomLight;
        }

        //Properties
        public int RoomID { get; init; }

        [Required]
        [MaxLength(30)]
        public string RoomName { get; set; }

        public int RoomSort { get; set; }

        [Required]
        public int RoomFloor { get; set; }

        public bool RoomLight { get; set; }

        public ICollection<Plant> Plants { get; set; }

        //override ToString function
        public override string ToString()
        {
            return $"RaumID: {RoomID}, Raumname: {RoomName}, Stockwerk: {RoomFloor}, viel Licht im Raum: {RoomLight}, Sortier-Nummer: {RoomSort}";
        }

    }
}
