using PlantGenius.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;


namespace PlantGeniusUserApp.GUI.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        //Datavariables
        private DataAccessLayer DAL;

        //private string _MainProperty;

        [ObservableProperty]
        private int count;

        [ObservableProperty]
        private string mainProperty;

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
        /// This method loads first all rooms and then load into each room the plants.
        /// virtuals: function can be overriden by child-classes
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
