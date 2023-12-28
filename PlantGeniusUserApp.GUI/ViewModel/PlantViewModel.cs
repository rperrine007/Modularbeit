﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PlantGenius.DAL;
using PlantGenius.DAL.Models;
using PlantGeniusUserApp.GUI.Views;

namespace PlantGeniusUserApp.GUI.ViewModel
{
    public partial class PlantsViewModel : ObservableObject, INotifyPropertyChanged
    {
        //Datavariables
        private DataAccessLayer DAL;

        //Properties
        public ObservableCollection<Plant> Plants { get; set; }
        public ICommand EditCommand { get; private set; }
        public ICommand WaterCommand { get; private set; }


        public PlantsViewModel()
        {
            // Initialize ObservableCollection
            Plants = new ObservableCollection<Plant>();
            EditCommand = new Command<Plant>(EditPlant);
            WaterCommand = new Command<Plant>(WaterPlant);
            DAL = new DataAccessLayer();
            LoadPlants();
        }


        /// <summary>
        /// Updates data when the user navigates to this page.
        /// This function seems buggy. Therefore it is obly used in the "Update" Button
        /// </summary>
        public ICommand PageAppearingCommand => new Command(async () =>
        {
            await LoadPlants();
        });

        public event PropertyChangedEventHandler? PropertyChanged;

        protected async void EditPlant(Plant plant)
        {
            if (plant != null)
            {
                var editViewModel = new PlantEditViewModel(plant);
                var editPage = new PlantPageEdit { BindingContext = editViewModel };

                await App.Current.MainPage.Navigation.PushAsync(editPage);
            }
        }


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected async void WaterPlant(Plant plant)
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

        private bool CanChangeToPlantPage()
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
            await App.Current.MainPage.Navigation.PushAsync(addPlantPage);
        }

    }
}