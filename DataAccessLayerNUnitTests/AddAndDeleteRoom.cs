using PlantGenius.GUI;
using System.Collections.ObjectModel;

namespace DataAccessLayerNUnitTests
{
    [TestFixture]
    public class AddAndDeleteRoom
    {

        private ObservableCollection<Room> roomList;
        private DatabaseConnector dbConnector;

        [SetUp]
        public void SetUp()
        {
            //create objects
            roomList = new ObservableCollection<Room>();
            dbConnector = new DatabaseConnector();
        }

        [Test]
        public async Task addAndDeleteRoom()
        {
            await DataAccessLayer.GetRooms(dbConnector, roomList);

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
            Assert.AreEqual(addedRoom.RoomName, lastRoom.RoomName, "Added RoomName is not equal.");
            Assert.AreEqual(lastRoom.RoomSortNumber, addedRoom.RoomSortNumber, "Added RoomSortNumber is not equal.");
            Assert.AreEqual(lastRoom.FloorOfRoom, addedRoom.FloorOfRoom, "Added FloorOfRoom is not equal.");
            Assert.AreEqual(lastRoom.RoomLight, addedRoom.RoomLight, "Added RoomLight is not equal.");

            await DataAccessLayer.DeleteRoomFromDB(dbConnector, roomList, lastRoom);

            await DataAccessLayer.GetRooms(dbConnector, roomList);
            lastRoom = roomList[roomList.Count - 1];


            //perform tests
            Assert.AreNotEqual(lastRoom.RoomName, addedRoom.RoomName, $"LastRoom name: {lastRoom.RoomName} should not be equal to {addedRoom.RoomName}");
            Assert.AreNotEqual(lastRoom.RoomSortNumber, addedRoom.RoomSortNumber, $"LastRoom sort number: {lastRoom.RoomSortNumber} should not be equal to {addedRoom.RoomSortNumber}");
        }
    }
}