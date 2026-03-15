using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using PlantGenius.DAL.Models;


namespace PlantGenius.DAL
{
    /// <summary>
    /// DB Context corresponds to a scheme in a DB. It sets the differents DbSets in relation to each other.
    /// </summary>
    public class AppDbContext : DbContext
    {

        private string connectionString = "";

        /// <summary>
        /// Constructor
        /// </summary>
        public AppDbContext(){}

        /// <summary>
        /// For The NUnit test I need another connection string to set.
        /// Modifies the DbContext to support an in-memory database
        /// </summary>
        /// <param name="options"></param>

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Overriden function to build options. Options are only build when there are not yet any.
        /// </summary>
        /// <param name="optionsBuilder"></param>
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