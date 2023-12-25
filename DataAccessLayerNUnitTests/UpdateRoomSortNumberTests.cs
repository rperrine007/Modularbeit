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
        public class UpdateRoomSortNumberTests
        {
            private AppDbContext context;
            private DataAccessLayer DALNUnit;
            private List<Room> rooms;

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
            public async Task UpdateRoomSortNumberTestTask()
            {
                //Add room for test
                var record = new Room()
                {
                    RoomName = "DALTest",
                    RoomSort = 3,
                    RoomFloor = -99,
                    RoomLight = false
                };

                await DALNUnit.AddRoomToDB(record);
                var addedRoom = context.Rooms.SingleOrDefault(x => x.RoomName == "DALTest");

                //Add second room for test
                var record2 = new Room()
                {
                    RoomName = "DALTest2",
                    RoomSort = 5,
                    RoomFloor = -99,
                    RoomLight = false
                };

                await DALNUnit.AddRoomToDB(record2);
                var addedRoom2 = context.Rooms.SingleOrDefault(x => x.RoomName == "DALTest2");

                //get rooms
                var rooms = await DALNUnit.GetRooms();
                //test retireved list 1st element
                Assert.AreEqual(record.RoomSort, rooms[0].RoomSort, "RoomSort Test unsuccessfull.");

                //test retrieved second element
                Assert.AreEqual(record2.RoomSort, rooms[1].RoomSort, "RoomSort 2 Test unsuccessfull.");

                //update sort number
                await DALNUnit.UpdateRoomSortNumber(addedRoom2.RoomID, 1);
                addedRoom2 = context.Rooms.SingleOrDefault(x => x.RoomName == "DALTest2");

                //perform test
                Assert.AreEqual(record2.RoomSort, 1, "RoomSort 2 Test unsuccessfull.");
                Assert.AreEqual(record2.RoomSort, addedRoom2.RoomSort, "RoomSort 2 Test unsuccessfull.");

                //test retireved list 1st element (should still be the same)
                Assert.AreEqual(record.RoomSort, rooms[0].RoomSort, "RoomSort Test unsuccessfull.");
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