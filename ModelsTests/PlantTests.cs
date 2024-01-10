using PlantGenius.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsNUnitTests
{
    // NUnit Tests of properties set through methods of the Plant class.
    [TestFixture]
    internal class PlantTests
    {
        private Plant TestPlant { get; set; }

        // Create an in memory DB
        [SetUp]
        public void SetUp()
        {
            TestPlant = new Plant()
            {
                PlantID = -100,
                PlantName = "Test",
                PlantNameScientific = "Test-Scientific",
                PlantWaterLastTime = DateTime.Today,
                PlantWaterRequirement = 10,
                RoomID = -1,
            };
        }

        [Test]
        public void indirectSetPropertyPlantWaterNextTimeTest()
        {
            string expectedString = DateTime.Today.AddDays(10).ToString("dd.MM.yyyy");
            Assert.That(TestPlant.PlantWaterNextTime, Is.EqualTo(expectedString), "Test of Property PlantWaterNextTime was not successfull");

        }

        [Test]
        public void indirectSetPropertyUpdatePlantSortTest()
        {
            int expectedNumber = (DateTime.Today.AddDays(10) - DateTime.Today).Days;
            Assert.That(TestPlant.PlantSort, Is.EqualTo(expectedNumber), "Test of Property PlantSort was not successfull");
        }


        [TearDown]
        public void TearDown() { }
    }
}
