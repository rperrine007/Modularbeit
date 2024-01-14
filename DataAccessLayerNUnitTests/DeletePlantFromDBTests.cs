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
    // NUnit Test if plants can be deleted correctly from the DB.
    [TestFixture]
    internal class DeletePlantFromDBTests
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
            DALNUnit = new DataAccessLayer(context);

            context.SaveChanges();

            //Add plant for test
            var record = new Plant()
            {
                PlantID = -100,
                PlantName = "Test",
                PlantNameScientific = "Test-Scientific",
                PlantWaterLastTime = DateTime.Today,
                PlantWaterRequirement = 10,
                RoomID = -1,
            };

            context.Plants.Add(record);
            context.SaveChanges();
        }

        [Test]
        public async Task DeletePlantTestTask()
        {
            var addedPlant = context.Plants.Single(x => x.PlantName == "Test");
            //Remove plant
            await DALNUnit.DeletePlantFromDB(addedPlant);

            // SingleOrDefault is a LINQ (Language-Integrated Query) method used to retrieve a single item from a sequence (like a DbSet in EF).
            // In this case it looks for a plant with the name "Test"
            var found = context.Plants.SingleOrDefault(x => x.PlantName == "Test");

            Assert.IsNull(found, "Deleted plant was found.");
        }

        //Delete the in memory DB
        [TearDown]
        public void TearDown()
        {
            context?.Dispose();
        }
    }
}
