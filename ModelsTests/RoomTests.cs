using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlantGenius.DAL.Models;


namespace ModelsNUnitTests
{
    [TestFixture]
    public class RoomTests
    {
        private Room TestRoom { get; set; }

        // Create an in memory DB
        [SetUp]
        public void SetUp()
        {
            TestRoom = new Room()
            {
                RoomID = -1,
                RoomSort = 99,
                RoomName = "Test",
                RoomFloor = -99,
                RoomLight = false,

            };
        }

        [Test]
        public void overrideToStringTest() {
            string expectedString = $"RaumID: {TestRoom.RoomID}, Raumname: {TestRoom.RoomName}, Stockwerk: {TestRoom.RoomFloor}, viel Licht im Raum: {TestRoom.RoomLight}, Sortier-Nummer: {TestRoom.RoomSort}";
            Assert.That(TestRoom.ToString(), Is.EqualTo(expectedString), "Test of the toString() method was not successfull");
        }

        [TearDown]
        public void TearDown() { }
    }
}
