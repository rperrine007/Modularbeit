using PlantGenius.DAL;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;


namespace PlantGeniusUserApp.GUI.ViewModel
{
    /// <summary>
    /// First page. Loads welcome page and displays a counter showing the number of plants that currently need water
    /// </summary>
    public partial class MainViewModel : ObservableObject
    {
        //Starting an instance for database
        private DataAccessLayer DAL;

        [ObservableProperty]
        private int count;

        [ObservableProperty]
        private string mainProperty;

        // Initialize the DAL
        public MainViewModel()
        {
            DAL = new DataAccessLayer();
        }

        //Async loading of CountPlantsToWater
        public async Task InitializeAsync()
        {
            await CountPlantsToWater();
        }


        /// <summary>
        /// This method loads all plants and shows a counter of plants that currently need water
        /// </summary>
        private async Task CountPlantsToWater()
        {
            var plants = await DAL.LoadPlantsFromDB();

            //Update PlantSortNumber
            foreach (var plant in plants)
            {
                plant.UpdatePlantSort();
            }

            // Sort the plants by PlantSort in ascending order
            var sortedPlants = plants.OrderBy(p => p.PlantSort);

            Count = 0;
            foreach (var plant in sortedPlants)
            {
                if(plant.PlantSort <= 0)
                {
                    Count++;
                }
            }
        }

        /// <summary>
        /// Updates data when the user navigates to this page.
        /// </summary>
        public ICommand PageAppearingCommand => new Command(async () => await CountPlantsToWater());
    }
}
