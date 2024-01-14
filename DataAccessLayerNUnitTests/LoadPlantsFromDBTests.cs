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
    internal class LoadPlantsFromDBTests
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

            //Add room for test
            var record = new Room()
            {
                RoomID = -12,
                RoomName = "DALTest",
                RoomSort = -999,
                RoomFloor = -99,
                RoomLight = false
            };

            context.Rooms.Add(record);
            context.SaveChanges();
        }

        [Test]
        public async Task LoadPlantsTestTask()
        {
            //Add plant for test
            var recordPlant = new Plant()
            {
                PlantName = "Test",
                PlantNameScientific = "Test-Scientific",
                PlantWaterLastTime = DateTime.Today,
                PlantWaterRequirement = 10,
                RoomID = -12,
            };

            //Add second plant for test
            var recordPlant2 = new Plant()
            {
                PlantName = "Test2",
                PlantNameScientific = "Test-Scientific2",
                PlantWaterLastTime = DateTime.Today,
                PlantWaterRequirement = 102,
                RoomID = -12,
            };

            await DALNUnit.AddPlantToDB(recordPlant);
            await DALNUnit.AddPlantToDB(recordPlant2);


            //get rooms
            var plants = await DALNUnit.LoadPlantsFromDB();

            //test retireved list 1st element
            Assert.That(plants[0].PlantName, Is.EqualTo(recordPlant.PlantName), "PlantName Test unsuccessfull.");
            Assert.That(plants[0].PlantID, Is.EqualTo(recordPlant.PlantID), "PlantID Test unsuccessfull.");
            Assert.That(plants[0].PlantNameScientific, Is.EqualTo(recordPlant.PlantNameScientific), "PlantNameScientific Test unsuccessfull.");
            Assert.That(plants[0].PlantWaterLastTime, Is.EqualTo(recordPlant.PlantWaterLastTime), "PlantWaterLastTime Test unsuccessfull.");
            Assert.That(plants[0].PlantWaterRequirement, Is.EqualTo(recordPlant.PlantWaterRequirement), "PlantWaterRequirement Test unsuccessfull.");
            Assert.That(plants[0].RoomID, Is.EqualTo(recordPlant.RoomID), "RoomID Test unsuccessfull.");

            //test retrieved second element
            Assert.That(plants[1].PlantName, Is.EqualTo(recordPlant2.PlantName), "PlantName2 Test unsuccessfull.");
            Assert.That(plants[1].PlantID, Is.EqualTo(recordPlant2.PlantID), "PlantID2 Test unsuccessfull.");
            Assert.That(plants[1].PlantNameScientific, Is.EqualTo(recordPlant2.PlantNameScientific), "PlantNameScientific2 Test unsuccessfull.");
            Assert.That(plants[1].PlantWaterLastTime, Is.EqualTo(recordPlant2.PlantWaterLastTime), "PlantWaterLastTime2 Test unsuccessfull.");
            Assert.That(plants[1].PlantWaterRequirement, Is.EqualTo(recordPlant2.PlantWaterRequirement), "PlantWaterRequirement2 Test unsuccessfull.");
            Assert.That(plants[1].RoomID, Is.EqualTo(recordPlant2.RoomID), "RoomID2 Test unsuccessfull.");
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
