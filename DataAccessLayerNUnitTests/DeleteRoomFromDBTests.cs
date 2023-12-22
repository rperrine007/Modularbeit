using Microsoft.EntityFrameworkCore;
using PlantGenius.DAL.Models;
using PlantGenius.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayerNUnitTests
{
    [TestFixture]
    internal class DeleteRoomFromDBTests
    {
        private AppDbContext context;

        // Create an in memory DB
        [SetUp]
        public void SetUp()
        {
            // returns the configured options of the DB. In this case it is a in memory database with the name "InMemoryDBForTesting + new unique identifier (G-UID)"- 
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: $"InMemoryDbForTesting{Guid.NewGuid()}").Options;
            context = new AppDbContext(options);

            context.SaveChanges();

            //Add room for test
            var record = new Room()
            {
                RoomName = "DALTest",
                RoomSort = -999,
                RoomFloor = -99,
                RoomLight = false
            };

            context.Rooms.Add(record);
            context.SaveChanges();
        }

        [Test]
        public void InsertRoom()
        {
            var addedRoom = context.Rooms.Single(x => x.RoomName == "DALTest");
            //Remove room
            context.Rooms.Remove(addedRoom);
            context.SaveChanges();

            // SingleOrDefault is a LINQ (Language-Integrated Query) method used to retrieve a single item from a sequence (like a DbSet in EF).
            // In this case it looks for a room with the name "DALTest"
            var found = context.Rooms.SingleOrDefault(x => x.RoomName == "DALTest");

            Assert.IsNull(found);
        }

        //Delete the in memory DB
        [TearDown]
        public void TearDown()
        {
            context?.Dispose();
        }
    }
}
