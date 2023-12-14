using PlantGenius.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace PlantGenius.DAL
{
    public class DataAccessLayer
    {
        public static async Task<(bool connectionStatus, string errorMessage)> TestDatabaseConnectionAsync()
        {
            try
            {
                using (var db = new AppDbContext())
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
        public static async Task<List<Room>> GetRooms()
        {
            while (true)
            {
                try
                {
                    using (var context = new AppDbContext())
                    {
                        return await context.Rooms.OrderBy(r => r.RoomSort).ToListAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching rooms: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                    return new List<Room>();
                }

            }
        }


        // Method to add a room
        public static async Task AddRoomToDB(Room roomInput)
        {
            using (var db = new AppDbContext())
            {
                db.Rooms.Add(roomInput);
                await db.SaveChangesAsync();
            }
        }

        // Method to update a room
        public static async Task UpdateRoomToDB(Room roomInput)
        {
            using (var db = new AppDbContext())
            {
                // Mark the room as modified
                db.Rooms.Attach(roomInput);
                db.Entry(roomInput).Property(r => r.RoomName).IsModified = true;
                db.Entry(roomInput).Property(r => r.RoomFloor).IsModified = true;
                db.Entry(roomInput).Property(r => r.RoomLight).IsModified = true;

                // Save changes to the database
                await db.SaveChangesAsync();

            }
        }

        // Method to delete a room
        public static async Task DeleteRoomFromDB(Room roomInput)
        {
            using (var db = new AppDbContext())
            {
                db.Rooms.Remove(roomInput);
                await db.SaveChangesAsync();
            }
        }

        // Method to update room sort number
        public static async Task UpdateRoomSortNumber(int roomId, int newSortNumber)
        {
            using (var db = new AppDbContext())
            {
                var room = await db.Rooms.FirstOrDefaultAsync(r => r.RoomID == roomId);
                if (room != null)
                {
                    room.RoomSort = newSortNumber;
                    await db.SaveChangesAsync();
                }
            }
        }

        //TODO implement this function in DAL to ensure that the roomsort number is continuous,
        /*
        /// <summary>
        /// Update the SortNumber of the rooms, when a room is deleted.
        /// </summary>
        /// <returns></returns>
        public async static Task OnRoomDeleteNewSort(DatabaseConnector_OLD dbConnectorInput, Collection<Room> roomListInput)
        {
            // Sort the Rooms by RoomSortNumber
            var sortedRooms = roomListInput.OrderBy(room => room.RoomSort).ToList();

            // Adding a new sorting number to each room to avoid gaps
            int newSortID = 1;
            foreach (var room in sortedRooms)
            {
                // Update Database
                using (var connection = await dbConnectorInput.GetDatabaseConnectionAsync())
                {
                    string query = $"UPDATE Room SET RoomSort = {newSortID} WHERE RoomID = {room.RoomID}";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
                newSortID++;
            }*/
        }
}
