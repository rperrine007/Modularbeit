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
    internal class UpdateRoomToDBTests
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
        public async Task UpdateRoomTestTask()
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

            //Add second room for test
            var record2 = new Room()
            {
                RoomName = "DALTest2",
                RoomSort = -999,
                RoomFloor = -99,
                RoomLight = false
            };

            await DALNUnit.AddRoomToDB(record2);
            var addedRoom2 = context.Rooms.SingleOrDefault(x => x.RoomName == "DALTest2");

            if (addedRoom != null)
            {
                //update room; it has to have the samre room as the record. Else it will not change it.
                addedRoom.RoomName = "DALUpdateTest";
                addedRoom.RoomSort = 999;
                addedRoom.RoomFloor = 99;
                addedRoom.RoomLight = true;

                //Update added Room
                await DALNUnit.UpdateRoomToDB(addedRoom);
                var originalAdddedRoom = context.Rooms.SingleOrDefault(x => x.RoomName == "DALTest");
                //added Room should not exist anymore
                Assert.IsNull(originalAdddedRoom, "Original room which should be updated was found.");

                //second added room should still exist
                Assert.IsNotNull(addedRoom2, "Second added room was not found.");

                //test updated room
                var updatedRoom = context.Rooms.SingleOrDefault(x => x.RoomName == "DALUpdateTest");
                if (updatedRoom != null)
                {
                    Assert.That(updatedRoom.RoomName, Is.EqualTo(record.RoomName), "RoomName Test unsuccessfull.");
                    Assert.That(updatedRoom.RoomSort, Is.EqualTo(record.RoomSort), "RoomSort Test unsuccessfull.");
                    Assert.That(updatedRoom.RoomFloor, Is.EqualTo(record.RoomFloor), "RoomFloor Test unsuccessfull.");
                    Assert.That(updatedRoom.RoomLight, Is.EqualTo(record.RoomLight), "RoomLight Test unsuccessfull.");
                }
                else
                {
                    Assert.IsNotNull(updatedRoom, "No updated room found.");
                }
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
            //remove items
            var todoItem = context.Rooms.Single(x => x.RoomName == "DALUpdateTest");
            context.Rooms.Remove(todoItem);
            var todoItem2 = context.Rooms.Single(x => x.RoomName == "DALTest2");
            context.Rooms.Remove(todoItem2);
            context.SaveChanges();
            //dispose inmemorydb
            context?.Dispose();
        }
    }
}