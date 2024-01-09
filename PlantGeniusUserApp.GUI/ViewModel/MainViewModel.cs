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
        public int count;

        [ObservableProperty]
        public string mainProperty;

        //public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            DAL = new DataAccessLayer();
            // Initialize commands
           // MainCommand = new Command(OnMainCommandExecuted);
            CountPlantsToWater();
        }


        /*
        public string MainProperty
        {
            get => _MainProperty;
            set
            {
                if (_MainProperty != value)
                {
                    _MainProperty = value;
                    OnPropertyChanged(nameof(MainProperty));
                }
            }
        }

        public ICommand MainCommand { get; }

        //TODO Für was war der MainComman gedacht? Ich finde keine Verwendung. Möglicherweise könnten wir ihn löschen?
        private void OnMainCommandExecuted()
        {
            // Command execution logic here
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        */

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
