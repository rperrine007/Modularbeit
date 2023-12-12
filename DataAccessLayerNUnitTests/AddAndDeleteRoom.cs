using PlantGenius.DAL;
using PlantGenius.DAL.Model;
using PlantGenius.GUI;
using System.Collections.ObjectModel;

namespace DataAccessLayerNUnitTests
{
    //[TestFixture]
    //public class AddAndDeleteRoom
    //{

        //private ObservableCollection<Room> roomList;

        //[SetUp]
        //public void SetUp()
        //{
        //    //create objects
        //    roomList = new ObservableCollection<Room>();
        //}

        //[Test]
        //public async Task addAndDeleteRoom()
        //{
        //    var initialRooms = await DataAccessLayer.GetRooms();
        //    foreach (var room in initialRooms)
        //    {
        //        roomList.Add(room);
        //    }

        //    Room addedRoom = new Room()
        //    {
        //        RoomName = "UnitTestRoom",
        //        RoomSort = roomList.Count + 1,
        //        RoomFloor = -3,
        //        RoomLight = true
        //    };

        //    await DataAccessLayer.AddRoomToDB(addedRoom);

        //    roomList.Clear();
        //    var roomsAfterAdd = await DataAccessLayer.GetRooms();
        //    foreach (var room in roomsAfterAdd)
        //    {
        //        roomList.Add(room);
        //    }

        //    Room lastRoom = roomList[roomList.Count - 1];

        //    //perform tests
        //    Assert.AreEqual(addedRoom.RoomName, lastRoom.RoomName, "Added RoomName is not equal.");
        //    Assert.AreEqual(lastRoom.RoomSort, addedRoom.RoomSort, "Added RoomSortNumber is not equal.");
        //    Assert.AreEqual(lastRoom.RoomFloor, addedRoom.RoomFloor, "Added FloorOfRoom is not equal.");
        //    Assert.AreEqual(lastRoom.RoomLight, addedRoom.RoomLight, "Added RoomLight is not equal.");

        //    await DataAccessLayer.DeleteRoomFromDB(lastRoom);

        //    roomList.Clear();
        //    var roomsAfterDelete = await DataAccessLayer.GetRooms();
        //    foreach (var room in roomsAfterDelete)
        //    {
        //        roomList.Add(room);
        //    }

        //    if (roomList.Count > 0)
        //    {
        //        lastRoom = roomList[roomList.Count - 1];

        //        // Perform tests
        //        Assert.AreNotEqual(lastRoom.RoomName, addedRoom.RoomName, $"LastRoom name: {lastRoom.RoomName} should not be equal to {addedRoom.RoomName}");
        //        Assert.AreNotEqual(lastRoom.RoomSort, addedRoom.RoomSort, $"LastRoom sort number: {lastRoom.RoomSort} should not be equal to {addedRoom.RoomSort}");
        //    }
        //}
    //}
}