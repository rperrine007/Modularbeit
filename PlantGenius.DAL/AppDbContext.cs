using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using PlantGenius.DAL.Model;


namespace PlantGenius.DAL
{
    public class AppDbContext : DbContext
    {
        public const string connectionString = "Server=49.12.196.20;Port=14501;Database=c1_zhaw2;User Id=c1_zhaw;Password=lQ9fKVoNK7ll!;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        public DbSet<Plant> Plants { get; set; }
        public DbSet<Room> Rooms { get; set; }
    }
}