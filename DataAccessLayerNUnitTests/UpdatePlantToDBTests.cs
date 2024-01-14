using Microsoft.EntityFrameworkCore;
using PlantGenius.DAL.Models;
using PlantGenius.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal;

namespace DataAccessLayerNUnitTests
{
    // NUnit Test if plants can be correctly updated to the DB.
    [TestFixture]
    internal class UpdatePlantToDBTests
    {
        private AppDbContext context;
        private DataAccessLayer DALNUnit;
        private Plant recordPlant;
        private Plant recordPlant2;

        // Create an in memory DB
        [SetUp]
        public void SetUp()
        {
            // returns the configured options of the DB. In this case it is a in memory database with the name "InMemoryDBForTesting + new unique identifier (G-UID)"- 
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: $"InMemoryDbForTesting{Guid.NewGuid()}").Options;
            context = new AppDbContext(options);
            context.SaveChanges();
            DALNUnit = new DataAccessLayer(context);


            //Add plant for test
            recordPlant = new Plant()
            {
                PlantName = "Test",
                PlantNameScientific = "Test-Scientific",
                PlantWaterLastTime = DateTime.Today,
                PlantWaterRequirement = 10,
                RoomID = -1,
            };

            //Add second plant for test
            recordPlant2 = new Plant()
            {
                PlantName = "Test2",
                PlantNameScientific = "Test-Scientific2",
                PlantWaterLastTime = DateTime.Today,
                PlantWaterRequirement = 102,
                RoomID = -12,
            };
        }

        [Test]
        public async Task UpdatePlantTestTask()
        {
            await DALNUnit.AddPlantToDB(recordPlant);
            await DALNUnit.AddPlantToDB(recordPlant2);

            var addedPlant = context.Plants.SingleOrDefault(x => x.PlantName == "Test");
            var addedPlant2 = context.Plants.SingleOrDefault(x => x.PlantName == "Test2");


            if (addedPlant != null)
            {
                //update plant; it has to have the samre plant as the record. Else it will not change it.
                addedPlant.PlantName = "DALUpdateTest";
                addedPlant.PlantNameScientific = "Test - Scientific Update";
                addedPlant.PlantWaterLastTime = DateTime.Today;
                addedPlant.PlantWaterRequirement = 20;
                addedPlant.RoomID = 20;

                //Update added plant
                await DALNUnit.UpdatePlantsToDB(addedPlant);
                context.SaveChanges();
                Plant? originalAddedPlant = context.Plants.SingleOrDefault(x => x.PlantName == "Test");

                //added plant should not exist anymore
                Assert.IsNull(originalAddedPlant, $"{originalAddedPlant} Original plant which should be updated was found.");

                //second added plant should still exist
                Assert.IsNotNull(addedPlant2, "Second added plant was not found.");

                //test updated plant
                var updatedPlant = context.Plants.SingleOrDefault(x => x.PlantName == "DALUpdateTest");
                if (updatedPlant != null)
                {
                    Assert.That(updatedPlant.PlantName, Is.EqualTo(addedPlant.PlantName), "PlantName Test unsuccessfull.");
                    Assert.That(updatedPlant.PlantID, Is.EqualTo(addedPlant.PlantID), "PlantID Test unsuccessfull.");
                    Assert.That(updatedPlant.PlantNameScientific, Is.EqualTo(addedPlant.PlantNameScientific), "PlantNameScientific Test unsuccessfull.");
                    Assert.That(updatedPlant.PlantWaterLastTime, Is.EqualTo(addedPlant.PlantWaterLastTime), "PlantWaterLastTime Test unsuccessfull.");
                    Assert.That(updatedPlant.PlantWaterRequirement, Is.EqualTo(addedPlant.PlantWaterRequirement), "PlantWaterRequirement Test unsuccessfull.");
                    Assert.That(updatedPlant.RoomID, Is.EqualTo(addedPlant.RoomID), "RoomID Test unsuccessfull.");
                }
                else
                {
                    Assert.IsNotNull(updatedPlant, "No updated plant found.");
                }
            }
            else
            {
                Assert.IsNotNull(addedPlant, "No added plant found.");
            }
        }

        [Test]
        public async Task UpdatePlantWaterLastTimeTestTask()
        {
            await DALNUnit.AddPlantToDB(recordPlant);

            var addedPlant = context.Plants.SingleOrDefault(x => x.PlantName == "Test");

            if (addedPlant != null)
            {
                //update plant; it has to have the samre plant as the record. Else it will not change it.
                addedPlant.PlantWaterLastTime = new DateTime(2023, 3, 22);

                //Update added plant
                await DALNUnit.UpdatePlantWaterLastTime(addedPlant.PlantID);

                //test updated plant
                var updatedPlant = context.Plants.SingleOrDefault(x => x.PlantName == "Test");
                if (updatedPlant != null)
                {
                    Assert.That(updatedPlant.PlantWaterRequirement, Is.EqualTo(addedPlant.PlantWaterRequirement), "PlantWaterRequirement Test unsuccessfull.");
                }
                else
                {
                    Assert.IsNotNull(updatedPlant, "No updated plant found.");
                }
            }
            else
            {
                Assert.IsNotNull(addedPlant, "No added plant found.");
            }
        }

        //Delete the in memory DB
        [TearDown]
        public void TearDown()
        {
            //remove items
            //var plant3 = context.Plants.Single(x => x.PlantName == "Test");
            //context.Plants.Remove(plant3);
            //context.SaveChanges();
            //dispose inmemorydb
            context?.Dispose();
        }
    }
}
