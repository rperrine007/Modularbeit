using Microsoft.EntityFrameworkCore;
using PlantGenius.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantGenius.DAL
{
    class AppDbContext : DbContext
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