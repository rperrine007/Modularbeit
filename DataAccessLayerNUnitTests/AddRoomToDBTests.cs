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
    public class AddRoomToDBTests
    {
        private AppDbContext context;
        private DataAccessLayer DALNUnit;

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
        public async Task InsertRoomTestTask()
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
            var addedRoom = context.Rooms.SingleOrDefault(x => x.RoomName == "DALTest");

            
            Assert.AreEqual(record.RoomName, addedRoom.RoomName, "RoomName Test unsuccessfull.");
            Assert.AreEqual(record.RoomSort, addedRoom.RoomSort, "RoomSort Test unsuccessfull.");
            Assert.AreEqual(record.RoomFloor, addedRoom.RoomFloor, "RoomFloor Test unsuccessfull.");
            Assert.AreEqual(record.RoomLight, addedRoom.RoomLight, "RoomLight Test unsuccessfull.");
        }

        //Delete the in memory DB
        [TearDown]
        public void TearDown()
        {
            var todoItem = context.Rooms.Single(x => x.RoomName == "DALTest");
            context.Rooms.Remove(todoItem);
            context.SaveChanges();
            context?.Dispose();
        }
    }
}