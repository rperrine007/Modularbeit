using PlantGenius.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Org.BouncyCastle.Asn1.Crmf;
using Microsoft.VisualBasic;
using System.Numerics;
using Google.Protobuf.WellKnownTypes;

namespace PlantGenius.DAL
{
    public class DataAccessLayer
    {
        private AppDbContext db;
        public DataAccessLayer() { 
            db =  new AppDbContext();
        }
        public DataAccessLayer(AppDbContext context) { 
            db = context;
        }

        // By problems with DB use this method.
        /*
        public async Task<(bool connectionStatus, string errorMessage)> TestDatabaseConnectionAsync()
        {
            try
            {
                using (db)
                {
                    // Attempt to fetch the first entity from some table.
                    var entity = await db.Rooms.FirstOrDefaultAsync();

                    if (entity != null)
                    {
                        Console.WriteLine("Success!");
                        return (true, "Connection successful");
                    }
                    else
                    {
                        return (false, "Connection failed to retrieve data");
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Database connection test failed: {ex.Message}");
            }
        }*/

        /// <summary>
        /// Method to get rooms.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Room>> GetRooms()
        {
                try
                {
                    return await db.Rooms.OrderBy(r => r.RoomSort).ToListAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"$\"Fehler bei download von Raum-Daten: : {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                }
            return new List<Room>();
        }


        /// <summary>
        /// Method to add a room.
        /// </summary>
        /// <param name="roomInput"></param>
        /// <returns></returns>
        public async Task AddRoomToDB(Room roomInput)
        {
                db.Rooms.Add(roomInput);
                await db.SaveChangesAsync();
        }

        /// <summary>
        /// Method to update rooms.
        /// </summary>
        /// <param name="roomInput"></param>
        /// <returns></returns>
        public async Task UpdateRoomToDB(Room roomInput)
        {
                // Mark the room as modified
                db.Rooms.Attach(roomInput);
                db.Entry(roomInput).Property(r => r.RoomName).IsModified = true;
                db.Entry(roomInput).Property(r => r.RoomFloor).IsModified = true;
                db.Entry(roomInput).Property(r => r.RoomLight).IsModified = true;

                // Save changes to the database
                await db.SaveChangesAsync();
        }

        /// <summary>
        /// Method to delete a room.
        /// </summary>
        /// <param name="roomInput"></param>
        /// <returns></returns>
        public async Task DeleteRoomFromDB(Room roomInput)
        {
             db.Rooms.Remove(roomInput);
            await db.SaveChangesAsync();
            //Refresh the RoomSort Number
            await RefreshSortRooms();
        }

        /// <summary>
        /// Method to update room sort number
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="newSortNumber"></param>
        /// <returns></returns>
        public async Task UpdateRoomSortNumber(int roomId, int newSortNumber)
        {
                var room = await db.Rooms.FirstOrDefaultAsync(r => r.RoomID == roomId);
                if (room != null)
                {
                    room.RoomSort = newSortNumber;
                    await db.SaveChangesAsync();
                }
        }

        ///summary>
        /// Update the SortNumber of the rooms, when a room is deleted.
        /// </summary>
        /// <returns></returns>
        public async Task RefreshSortRooms()
        {
            List<Room> sortedRooms = null;

            try
            {
                    // Sort the Rooms by RoomSortNumber
                    sortedRooms = await db.Rooms.OrderBy(r => r.RoomSort).ToListAsync();
                    // Adding a new sorting number to each room to avoid gaps
                    int newSortID = 1;
                    foreach (var room in sortedRooms)
                    {
                        room.RoomSort = newSortID;
                        await db.SaveChangesAsync();
                        // Update Database
                        newSortID++;
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching rooms: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// This method loads first all rooms and then load into each room the plants.
        /// virtuals: function can be overriden by child-classes
        /// </summary>
        public async Task<List<Plant>> LoadPlantsFromDB()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    // Async database operations
                    return await db.Plants.OrderBy(r => r.PlantSort).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei download von Pflanzen-Daten: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
            return new List<Plant>();
        }

        // Method to update PlantWaterLastTime
        public async Task UpdatePlantWaterLastTime(int plantId)
        {
            using (var db = new AppDbContext())
            {
                var plant = await db.Plants.FirstOrDefaultAsync(r => r.PlantID == plantId);
                if (plant != null)
                {
                    plant.PlantWaterLastTime = DateTime.Today;
                    await db.SaveChangesAsync();
                }
            }
        }

        // Method to update a plant
        public async Task UpdatePlantsToDB(Plant plantInput)
        {
            // Mark the plant as modified
            //TODO Perrine the room is not stored correctly
            db.Plants.Attach(plantInput);
            db.Entry(plantInput).Property(r => r.PlantName).IsModified = true;
            db.Entry(plantInput).Property(r => r.PlantNameScientific).IsModified = true;
            db.Entry(plantInput).Property(p => p.PlantRoom).IsModified = true;
            db.Entry(plantInput).Property(r => r.PlantSort).IsModified = true;
            db.Entry(plantInput).Property(r => r.PlantWaterRequirement).IsModified = true;
            db.Entry(plantInput).Property(r => r.PlantWaterLastTime).IsModified = true;

            // Save changes to the database
            await db.SaveChangesAsync();
        }


    }
}
