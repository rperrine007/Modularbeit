using Microsoft.EntityFrameworkCore;
using PlantGenius.DAL;
using PlantGenius.DAL.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayerNUnitTests
{
    [TestFixture]
    public class GetRoomsFromDB
    {
        private AppDbContext context;
        private DataAccessLayer DALNUnit;
        private List<Room> rooms;

        // Create an in memory DB
        [SetUp]
        public void SetUp()
        {
            // returns the configured options of the DB. In this case it is a in memory database with the name "InMemoryDBForTesting + new unique identifier (G-UID)"- 
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: $"InMemoryDbForTesting{Guid.NewGuid()}").Options;
            context = new AppDbContext(options);
            context.SaveChanges();
            DALNUnit = new DataAccessLayer(context);
        }

        [Test]
        public async Task GetRoomsTestTask()
        {
            //Add room for test
            var record = new Room()
            {
                RoomName = "DALTest",
                RoomSort = -999,
                RoomFloor = -99,
                RoomLight = false
            };

            await DALNUnit.AddRoomToDB(record);

            //Add second room for test
            var record2 = new Room()
            {
                RoomName = "DALTest2",
                RoomSort = -999,
                RoomFloor = -99,
                RoomLight = false
            };

            await DALNUnit.AddRoomToDB(record2);


            //get rooms
            var rooms = await DALNUnit.GetRooms();

            //test retireved list 1st element
            Assert.AreEqual(record.RoomName, rooms[0].RoomName, "RoomName Test unsuccessfull.");
            Assert.AreEqual(record.RoomSort, rooms[0].RoomSort, "RoomSort Test unsuccessfull.");
            Assert.AreEqual(record.RoomFloor, rooms[0].RoomFloor, "RoomFloor Test unsuccessfull.");
            Assert.AreEqual(record.RoomLight, rooms[0].RoomLight, "RoomLight Test unsuccessfull.");

            //test retrieved second element
            Assert.AreEqual(record2.RoomName, rooms[1].RoomName, "RoomName 2 Test unsuccessfull.");
            Assert.AreEqual(record2.RoomSort, rooms[1].RoomSort, "RoomSort 2 Test unsuccessfull.");
            Assert.AreEqual(record2.RoomFloor, rooms[1].RoomFloor, "RoomFloor 2 Test unsuccessfull.");
            Assert.AreEqual(record2.RoomLight, rooms[1].RoomLight, "RoomLight 2 Test unsuccessfull.");
        }

        //Delete the in memory DB
        [TearDown]
        public void TearDown()
        {
            //dispose inmemorydb
            context?.Dispose();
        }
    }
}