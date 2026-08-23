using System;
using System.Linq;
using PlantGenius.DAL.Models;

namespace PlantGenius.DAL
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // DB erstellen, falls noch nicht vorhanden
            context.Database.EnsureCreated();

            // Prüfen, ob schon Räume existieren
            if (!context.Rooms.Any())
            {
                // Beispiel-Raum anlegen
                var livingRoom = new Room
                {
                    RoomName = "Wohnzimmer",
                    RoomFloor = 1,
                    RoomLight = true,
                    RoomSort = 1
                };
                context.Rooms.Add(livingRoom);

                // Beispiel-Pflanzen anlegen
                context.Plants.AddRange(
                    new Plant
                    {
                        PlantName = "Aloe Vera",
                        PlantNameScientific = "Aloe barbadensis",
                        RoomID = livingRoom.RoomID,
                        PlantWaterRequirement = 7,
                        PlantWaterLastTime = DateTime.Today.AddDays(-3)
                    },
                    new Plant
                    {
                        PlantName = "Monstera",
                        PlantNameScientific = "Monstera deliciosa",
                        RoomID = livingRoom.RoomID,
                        PlantWaterRequirement = 10,
                        PlantWaterLastTime = DateTime.Today.AddDays(-11)
                    }
                );

                context.SaveChanges();
            }
        }
    }
}