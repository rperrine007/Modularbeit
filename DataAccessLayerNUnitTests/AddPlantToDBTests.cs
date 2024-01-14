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
    // NUnit Test if plants can be correctly loaded from the DB.
    [TestFixture]
    internal class AddPlantToDBTests
    {

        private AppDbContext context;
        private DataAccessLayer DALNUnit;
        private Plant? AddedPlant { get; set; }

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
        public async Task AddPlantsTestTask()
        {

            //Add room for test
            var recordPlant = new Plant()
            {
                PlantID = -100,
                PlantName = "Test",
                PlantNameScientific = "Test-Scientific",
                PlantWaterLastTime = DateTime.Today,
                PlantWaterRequirement = 10,
                RoomID = -1,
            };

            await DALNUnit.AddPlantToDB(recordPlant);

            AddedPlant = context.Plants.SingleOrDefault(x => x.PlantName == "Test");

            if (AddedPlant != null)
            {
                Assert.That(AddedPlant.PlantName, Is.EqualTo(recordPlant.PlantName), "PlantName Test unsuccessfull.");
                Assert.That(AddedPlant.PlantID, Is.EqualTo(recordPlant.PlantID), "PlantID Test unsuccessfull.");
                Assert.That(AddedPlant.PlantNameScientific, Is.EqualTo(recordPlant.PlantNameScientific), "PlantNameScientific Test unsuccessfull.");
                Assert.That(AddedPlant.PlantWaterLastTime, Is.EqualTo(recordPlant.PlantWaterLastTime), "PlantWaterLastTime Test unsuccessfull.");
                Assert.That(AddedPlant.PlantWaterRequirement, Is.EqualTo(recordPlant.PlantWaterRequirement), "PlantWaterRequirement Test unsuccessfull.");
                Assert.That(AddedPlant.RoomID, Is.EqualTo(recordPlant.RoomID), "RoomID Test unsuccessfull.");
            }
            else
            {
                Assert.IsNotNull(AddedPlant, "No added plant found.");
            }

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
