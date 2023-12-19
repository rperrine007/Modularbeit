using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PlantGenius.DAL;
using PlantGenius.DAL.Models;

namespace PlantGeniusUser.GUI.ViewModel
{
    public class PlantsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Plant> Plants { get; set; }
        public ICommand EditCommand { get; private set; }
        public ICommand WaterCommand { get; private set; }

        public PlantsViewModel()
        {
            // Initialize ObservableCollection
            Plants = new ObservableCollection<Plant>();
            EditCommand = new Command<Plant>(EditPlant);
            WaterCommand = new Command<Plant>(WaterPlant);

            // Now get the plants from database
            //LoadPlants();
            LoadDemoData();
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

        private void LoadPlants()
        {
            try
            {
                using (var dbContext = new AppDbContext())
                {
                    var plants = dbContext.Plants
                        .Include(p => p.Room)
                        .ToList();

                    // Add items to the collection
                    foreach (var plant in plants)
                    {
                        Plants.Add(plant);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }

        }

        private void LoadDemoData()
        {
            // Create and add demo plants
            Plants.Add(new Plant("Rose", DateTime.Now, 7));
            Plants.Add(new Plant("Kaktus", DateTime.Now, 5));
            Plants.Add(new Plant("Testblume", DateTime.Now, 10));
        }

    }
}