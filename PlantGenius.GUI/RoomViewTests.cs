using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PlantGenius.GUI
{
    [TestFixture]
    public class RoomViewTests
    {

        private ObservableCollection<Room> roomList;
        private DatabaseConnector dbConnector;
         
        [SetUp]
        public async void SetUp()
        {
            //create objects
            roomList = new ObservableCollection<Room>();
            dbConnector = new DatabaseConnector();
        }

       [Test]       public async Task addAndDeleteRoom()
        {
            Room addedRoom = new Room()
            {
                RoomID = -1,
                RoomName = "UnitTestRoom",
                RoomSortNumber = roomList.Count + 1,
                FloorOfRoom = -3,
                RoomLight = true
            };

            await DataAccessLayer.AddRoomToDB(dbConnector, addedRoom);
            await DataAccessLayer.GetRooms(dbConnector, roomList);

            Room lastRoom = roomList[roomList.Count - 1];

            //perform tests
            Assert.AreEqual(lastRoom.RoomName, addedRoom.RoomName);
            Assert.AreEqual(lastRoom.RoomSortNumber, addedRoom.RoomSortNumber);
            Assert.AreEqual(lastRoom.FloorOfRoom, addedRoom.FloorOfRoom);
            Assert.AreEqual(lastRoom.RoomLight, addedRoom.RoomLight);

            await DataAccessLayer.DeleteRoomFromDB(dbConnector, roomList, lastRoom);

            
            //perform tests
            Assert.AreNotEqual(lastRoom.RoomName, addedRoom.RoomName);
            Assert.AreNotEqual(lastRoom.RoomSortNumber, addedRoom.RoomSortNumber);
            Assert.AreNotEqual(lastRoom.FloorOfRoom, addedRoom.FloorOfRoom);
            Assert.AreNotEqual(lastRoom.RoomLight, addedRoom.RoomLight);
        }



    }
}
