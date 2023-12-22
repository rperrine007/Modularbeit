using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using PlantGenius.DAL.Models;


namespace PlantGenius.DAL
{
    //DB Context corresponds to a scheme in a DB. It sets the differents DbSets in relation to each other.
    public class AppDbContext : DbContext
    {

        private string connectionString = "Server=49.12.196.20;Port=14501;Database=c1_zhaw2;User Id=c1_zhaw;Password=lQ9fKVoNK7ll!;";

        //Constructors
        public AppDbContext(){}

        //For The NUnit test I need another connection string to set.
        //Modifies the DbContext to support an in-memory database
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //To create a in memory DB this function should not be called as the options will already be build. 
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }

        // DB Set corresponds to a table in the DB. It has a lot of useful functions but can cause performance problems.
        // necessary DBSet-Properties.
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Room> Rooms { get; set; }
    }
}