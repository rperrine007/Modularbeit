﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantGenius.DAL;
using PlantGenius.DAL.Models;
using PlantGeniusUserApp.GUI.Views;

namespace PlantGeniusUserApp.GUI.ViewModel
{
    /// <summary>
    /// Shows the plant overview page - plants are sorted by next water date. Press and hold to edit or delete the plants. The plant can be watered by pressing the watering can.
    /// </summary>
    public partial class PlantsViewModel : ObservableObject
    {
        //Datavariables
        private DataAccessLayer DAL;

        //Properties
        public ObservableCollection<Plant> Plants { get; set; }

        public PlantsViewModel()
        {
            // Initialize ObservableCollection
            Plants = new ObservableCollection<Plant>();
            DAL = new DataAccessLayer();
            UpdatePlants();
        }


        /// <summary>
        /// Updates data when the user navigates to this page.
        /// </summary>
        protected ICommand PageAppearingCommand => new Command(async () => await UpdatePlants());

        protected bool CanUpdatePlants()
        {
            return true;
        }

        /// <summary>
        /// Updates data when the user navigates to this page.
        /// Delay added so that the button is for a short time not clickable.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanUpdatePlants))]
        protected async Task UpdatePlants()
        {
            await LoadPlants();
            await Task.Delay(1000);
        }

        protected bool CanEditPlant()
        {
            return true;
        }

        [RelayCommand(CanExecute = nameof(CanEditPlant))]
        protected async Task EditPlant(Plant plant)
        {
            if (plant != null)
            {
                var editViewModel = new PlantEditViewModel(plant);
                var editPage = new PlantPageEdit { BindingContext = editViewModel };

                if (App.Current != null && App.Current.MainPage != null) await App.Current.MainPage.Navigation.PushAsync(editPage);
                
            }
        }       

        protected bool CanWaterPlant()
        {
            return true;
        }

        [RelayCommand(CanExecute = nameof(CanWaterPlant))]
        protected async Task WaterPlant(Plant plant)
        {
            if (plant != null)
            {
                // Await the asynchronous database update operation
                await DAL.UpdatePlantWaterLastTime(plant.PlantID);
                await LoadPlants();
            }
        }

        /// <summary>
        /// This method loads first all rooms and then load into each room the plants.
        /// virtuals: function can be overriden by child-classes
        /// </summary>
        protected virtual async Task LoadPlants()
        {
            Plants.Clear();
            var plants = await DAL.LoadPlantsFromDB();
            //Update PlantSortNumber
            foreach (var plant in plants)
            {
                plant.UpdatePlantSort();
            }

            // Sort the plants by PlantSort in ascending order
            var sortedPlants = plants.OrderBy(p => p.PlantSort);

            foreach (var plant in sortedPlants)
            {
                Plants.Add(plant);
            }
        }

        protected bool CanChangeToPlantPage()
        {
            return true;
        }

        /// <summary>
        /// Adds a new room to the roomList and the DB.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanChangeToPlantPage))]
        protected async Task ChangeToPlantPage()
        {
            var addPlantPage = new AddPlantPage();
            if (App.Current != null && App.Current.MainPage != null) await App.Current.MainPage.Navigation.PushAsync(addPlantPage);
        }

        protected bool CanDelete()
        {
            return true;
        }

        /// <summary>
        /// Adds a new room to the roomList and the DB.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanDelete))]
        protected async Task Delete(Plant selectedPlant)
        {
            // Ask again if the user really wants to delete the selected plant.
            string AlertDescription = "Bist du sicher, dass du diese Pflanze löschen möchtest?";
            if (App.Current != null && App.Current.MainPage != null)
            {
                bool answer = await App.Current.MainPage.DisplayAlert("Warning", AlertDescription, "Ja", "Nein");

                if (answer)
                {
                    await DAL.DeletePlantFromDB(selectedPlant);
                    await UpdatePlants();
                }
                else
                {
                    return;
                }
            }
        }

    }
}