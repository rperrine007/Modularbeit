using PlantGenius.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace PlantGenius.DAL
{
    public class DataAccessLayer
    {
        // Method to get rooms
        public static async Task<List<Room>> GetRooms()
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


        // Method to add a room
        public static async Task AddRoomToDB(Room roomInput)
        {
            using (var db = new AppDbContext())
            {
                db.Rooms.Add(roomInput);
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
    }
}
