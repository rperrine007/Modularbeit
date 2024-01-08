﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Layouts;
using Org.BouncyCastle.Tls;
using PlantGenius.DAL;
using PlantGenius.DAL.Models;
using PlantGeniusUserApp.GUI.Views;

namespace PlantGeniusUserApp.GUI.ViewModel
{
    public partial class PlantsViewModel : ObservableObject, INotifyPropertyChanged
    {
        //Datavariables
        private DataAccessLayer DAL;

        //Observable Property Update-Button enabled/disabled
        //TODO Why did the [Observable Property] not work here?

        private bool isUpdateButtonEnabled;
        
        public bool IsUpdateButtonEnabled
        {
            get => isUpdateButtonEnabled;
            set
            {
                if (isUpdateButtonEnabled != value)
                {
                    isUpdateButtonEnabled = value;
                    OnPropertyChanged(nameof(IsUpdateButtonEnabled));
                }
            }
        }

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
            IsUpdateButtonEnabled = false;
        }


        /// <summary>
        /// Updates data when the user navigates to this page.
        /// </summary>
        public ICommand PageAppearingCommand => new Command(async () => await UpdatePlants());


        private bool CanUpdatePlants()
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
            IsUpdateButtonEnabled = false;
            await LoadPlants();
            await Task.Delay(1000);
            IsUpdateButtonEnabled = true;
        }
                

        protected async void EditPlant(Plant plant)
        {
            if (plant != null)
            {
                var editViewModel = new PlantEditViewModel(plant);
                var editPage = new PlantPageEdit { BindingContext = editViewModel };

                if (App.Current != null && App.Current.MainPage != null) await App.Current.MainPage.Navigation.PushAsync(editPage);
                
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

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
            if (App.Current != null && App.Current.MainPage != null) await App.Current.MainPage.Navigation.PushAsync(addPlantPage);
        }

        private bool CanDelete()
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