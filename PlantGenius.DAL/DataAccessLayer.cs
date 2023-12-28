using PlantGenius.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Org.BouncyCastle.Asn1.Crmf;
using Microsoft.VisualBasic;
using System.Numerics;
using Google.Protobuf.WellKnownTypes;

namespace PlantGenius.DAL
{
    /// <summary>
    /// In this class all methods are stored with which data from the backend of the application to the DB is transfered.
    /// </summary>
    public class DataAccessLayer
    {
        // datavaribale db which will contain the connection to the DB. 
        private AppDbContext db;

        /// <summary>
        /// Constructor, the connection string of db is in AppDbContext contained.
        /// </summary>
        public DataAccessLayer()
        {
            db = new AppDbContext();
        }

        /// <summary>
        /// Constructor if the context of the DB is ^given. With this e.g. a InMemoryDataBase can be connected.
        /// </summary>
        /// <param name="context"></param>
        public DataAccessLayer(AppDbContext context)
        {
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
        /// Then a list of the rooms with plants is given back.
        /// </summary>
        public async Task<HashSet<int>> GetRoomsWithPlants()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    // Async database operations
                    var rooms = await db.Rooms.OrderBy(r => r.RoomSort).ToListAsync();
                    var roomIDList = new HashSet<int>();

                    foreach (var room in rooms)
                    {

                        var plantsInRoom = await db.Plants
                                     .Where(p => p.RoomID == room.RoomID)
                                     .Include(p => p.Room)
                                     .ToListAsync();

                        foreach (var plant in plantsInRoom)
                        {
                            roomIDList.Add(plant.Room.RoomID);
                        }

                    }
                    return roomIDList;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei download von Pflanzen-Daten: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
            return new HashSet<int>();
        }

        /// <summary>
        /// This method loads first all rooms and then load into each room the plants.
        /// </summary>
        public async Task<List<Plant>> LoadPlantsFromDB()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    // Async database operations
                    var rooms = await db.Rooms.OrderBy(r => r.RoomSort).ToListAsync();
                    List<Plant> plantList = new List<Plant>();

                    foreach (var room in rooms)
                    {

                        var plantsInRoom = await db.Plants
                                     .Where(p => p.RoomID == room.RoomID)
                                     .Include(p => p.Room)
                                     .ToListAsync();

                        foreach (var plant in plantsInRoom)
                        {
                            plantList.Add(plant);
                        }

                    }
                    return plantList;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei download von Pflanzen-Daten: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
            return new List<Plant>();
        }

        /// <summary>
        /// Method to add a room.
        /// </summary>
        /// <param name="roomInput"></param>
        /// <returns></returns>
        public async Task AddPlantToDB(Plant plantInput)
        {
            db.Plants.Add(plantInput);
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Method to update PlantWaterLastTime.
        /// </summary>
        /// <param name="plantId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method to update a plant.
        /// </summary>
        /// <param name="plantInput"></param>
        /// <returns></returns>
        public async Task UpdatePlantsToDB(Plant plantInput)
        {
            // Assuming updatedPlant is passed with the new RoomID set
            var existingPlant = await db.Plants
                                        .Include(p => p.Room) // Include Room if navigation property exists
                                        .FirstOrDefaultAsync(p => p.PlantID == plantInput.PlantID);

            if (existingPlant != null)
            {
                // Update properties
                existingPlant.PlantName = plantInput.PlantName;
                existingPlant.PlantNameScientific = plantInput.PlantNameScientific;
                existingPlant.RoomID = plantInput.RoomID;
                existingPlant.PlantSort = plantInput.PlantSort;
                existingPlant.PlantWaterRequirement = plantInput.PlantWaterRequirement;
                existingPlant.PlantWaterLastTime = plantInput.PlantWaterLastTime;

                // Mark as modified and save
                db.Plants.Update(existingPlant);

                // Save changes to the database
                await db.SaveChangesAsync();
            }


        }
    }
}
