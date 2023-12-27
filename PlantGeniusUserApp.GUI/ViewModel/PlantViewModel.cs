﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PlantGenius.DAL;
using PlantGenius.DAL.Models;
using PlantGeniusUserApp.GUI.Views;

namespace PlantGeniusUserApp.GUI.ViewModel
{
    public class PlantsViewModel : INotifyPropertyChanged
    {
        //Datavariables
        private DataAccessLayer DAL;

        //Properties
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
            DAL = new DataAccessLayer();

            // Now get the plants from database
            LoadPlants();
        }

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
                UpdatePlantList();
            }
        }


        /// <summary>
        /// This method loads first all rooms and then load into each room the plants.
        /// virtuals: function can be overriden by child-classes
        /// </summary>
        protected virtual async Task LoadPlants()
        { 
            var plants = await DAL.LoadPlantsFromDB();
            Plants.Clear();
            foreach (var plant in plants)
            {
                Plants.Add(plant);
            }
        }


        protected virtual async void UpdatePlantList()
        {
            Plants.Clear();
            await LoadPlants();
        }


    }
}