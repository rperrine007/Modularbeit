using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PlantGenius.DAL;
using PlantGenius.DAL.Models;

namespace PlantGeniusUserApp.GUI.ViewModel
{
    public class PlantsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Plant> Plants { get; set; }
        public ICommand EditCommand { get; private set; }
        public ICommand WaterCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }

        public PlantsViewModel()
        {
            // Initialize ObservableCollection
            Plants = new ObservableCollection<Plant>();
            EditCommand = new Command<Plant>(EditPlant);
            WaterCommand = new Command<Plant>(WaterPlant);
            UpdateCommand = new Command(UpdatePlantList);

            // Now get the plants from database
            LoadPlants();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void EditPlant(Plant plant)
        {
            // 
        }

        private void WaterPlant(Plant plant)
        {
            // 
        }

        /// <summary>
        /// This method loads first all rooms and then load into each room the plants
        /// </summary>
        private void LoadPlants()
        {
            try
            {
                using (var dbContext = new AppDbContext())
                {
                    // Get all rooms
                    var rooms = dbContext.Rooms
                                         .OrderBy(r => r.RoomSort)
                                         .ToList();

                    foreach (var room in rooms)
                    {
                        // Get all plants for each room
                        var plantsInRoom = dbContext.Plants
                                                    .Where(p => p.PlantRoom == room.RoomID)
                                                    .Include(p => p.Room)
                                                    .OrderBy(p => p.PlantSort)
                                                    .ToList();

                        foreach (var plant in plantsInRoom)
                        {
                            Plants.Add(plant);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }

        private void UpdatePlantList()
        {
            Plants.Clear();
            LoadPlants();

        }
    }
}
