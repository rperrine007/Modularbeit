using Microsoft.EntityFrameworkCore;
using PlantGenius.DAL.Models;
using PlantGenius.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataAccessLayerNUnitTests
{
    // Test if rooms with plants can be recognized.
    [TestFixture]
    internal class GetRoomsWithPlantsTests
    {
         
        private AppDbContext context;
        private DataAccessLayer DALNUnit;
        private Room record;
        private Room record2;

        // Create an in memory DB
        [SetUp]
        public void SetUp()
        {
            // returns the configured options of the DB. In this case it is a in memory database with the name "InMemoryDBForTesting + new unique identifier (G-UID)"- 
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: $"InMemoryDbForTesting{Guid.NewGuid()}").Options;
            context = new AppDbContext(options);
            context.SaveChanges();
            DALNUnit = new DataAccessLayer(context);

            //Add room for test
            record = new Room()
            {
                RoomID = -12,
                RoomName = "DALTest",
                RoomSort = -999,
                RoomFloor = -99,
                RoomLight = false
            };

            //Add room2 for test
            record2 = new Room()
            {
                RoomID = -122,
                RoomName = "DALTest2",
                RoomSort = -9992,
                RoomFloor = -992,
                RoomLight = false
            };

            context.Rooms.Add(record);
            context.Rooms.Add(record2);
            context.SaveChanges();

            //Add plant for test
            var recordPlant = new Plant()
            {
                PlantName = "Test",
                PlantNameScientific = "Test-Scientific",
                PlantWaterLastTime = DateTime.Today,
                PlantWaterRequirement = 10,
                RoomID = -12,
            };

            context.Plants.Add(recordPlant);
            context.SaveChanges();
        }

        [Test]
        public async Task LoadPlantsTestTask()
        {
            //get rooms
            var roomIDsWithPlants = await DALNUnit.GetRoomsWithPlants();

            //only delete room when no plants are contained.
            var roomWithPlants = roomIDsWithPlants.Contains(record.RoomID);
            var roomWithoutPlants = roomIDsWithPlants.Contains(record2.RoomID);

            Assert.IsTrue(roomWithPlants, "Room should have plants.");
            Assert.IsFalse(roomWithoutPlants, "Room should not have plants.");
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
