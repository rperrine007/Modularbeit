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
    public class AddRoomTests
    {
        private AppDbContext context;

        [SetUp]
        public void SetUp()
        {
            context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Rooms;Trusted_Connection=True;MultipleActiveResultSets=true").Options);
        }

        [Test]
        public void InsertRoom()
        {
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

            var addedRoom = context.Rooms.Single(x => x.RoomName == "DALTest");

            Assert.Equals(record.RoomName, addedRoom.RoomName);
            Assert.Equals(record.RoomSort, addedRoom.RoomSort);
            Assert.Equals(record.RoomFloor, addedRoom.RoomFloor);
            Assert.Equals(record.RoomLight, addedRoom.RoomLight);
        }

        [TearDown]
        public void TearDown()
        {
            var todoItem = context.Rooms.Single(x => x.RoomName == "DALTest");
            context.Rooms.Remove(todoItem);
            context.SaveChanges();
            context?.Dispose();
        }
    }
}