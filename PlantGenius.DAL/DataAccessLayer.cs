using PlantGenius.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Org.BouncyCastle.Asn1.Crmf;
using Microsoft.VisualBasic;

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
        }

        // Method to get rooms
        public async Task<List<Room>> GetRooms()
        {
                try
                {
                        await RefreshSortRooms();
                        return await db.Rooms.OrderBy(r => r.RoomSort).ToListAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching rooms: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                    return new List<Room>();
                }            
        }


        // Method to add a room
        public async Task AddRoomToDB(Room roomInput)
        {
                db.Rooms.Add(roomInput);
                await db.SaveChangesAsync();
        }

        // Method to update a room
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

        // Method to delete a room
        public async Task DeleteRoomFromDB(Room roomInput)
        {
             db.Rooms.Remove(roomInput);
            await db.SaveChangesAsync();
            //Refresh the RoomSort Number
            await RefreshSortRooms();
        }

        // Method to update room sort number
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

        // Method to update PlantWaterLastTime
        public static async Task UpdatePlantWaterLastTime(int plantId)
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


    }
}
