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
    // NUnit Test if room can be added correctly to the DB.
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

            if (addedRoom != null)
            {
                Assert.That(addedRoom.RoomName, Is.EqualTo(record.RoomName), "RoomName Test unsuccessfull.");
                Assert.That(addedRoom.RoomSort, Is.EqualTo(record.RoomSort), "RoomSort Test unsuccessfull.");
                Assert.That(addedRoom.RoomFloor, Is.EqualTo(record.RoomFloor), "RoomFloor Test unsuccessfull.");
                Assert.That(addedRoom.RoomLight, Is.EqualTo(record.RoomLight), "RoomLight Test unsuccessfull.");
            }
            else
            {
                Assert.IsNotNull(addedRoom, "No added room found.");
            }
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