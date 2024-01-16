using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Org.BouncyCastle.Tls;
using PlantGenius.DAL;
using PlantGenius.DAL.Models;
using static System.Net.Mime.MediaTypeNames;

namespace PlantGeniusUserApp.GUI.ViewModel
{
    /// <summary>
    /// Edit view to edit existing plants
    /// </summary>
    public partial class PlantEditViewModel : ObservableObject
    {
        public ObservableCollection<Room> Rooms { get; private set; }

        [ObservableProperty]
        private Room selectedRoom;

        [ObservableProperty]
        private Plant selectedPlant;

        // Adding a copy of the plant object to avoid writing to the original object before storing
        private Plant copyPlant;

        public String AlertDescription { get; set; }
        public PlantEditViewModel(Plant selectedPlant)
        {
            SelectedPlant = selectedPlant;

            // Adding a copy of the original object
            copyPlant = new Plant
            {
                PlantName = selectedPlant.PlantName,
                PlantNameScientific = selectedPlant.PlantNameScientific,
                PlantID = selectedPlant.PlantID,
                PlantSort = selectedPlant.PlantSort,
                PlantWaterLastTime = selectedPlant.PlantWaterLastTime,
                PlantWaterRequirement = selectedPlant.PlantWaterRequirement
            };

            // Now get the rooms from database and insert in collection
            Rooms = new ObservableCollection<Room>();
            LoadRooms();
        }

        /// <summary>
        /// Check if the Plant can be stored with the entered input
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanSavePlant(object obj)
        {
            return true;
        }

        //TODO Plant will always be stored without save also when returning with back
        [RelayCommand(CanExecute = nameof(CanSavePlant))]
        private async Task SavePlant(object obj)
        {
            if (copyPlant == null || string.IsNullOrEmpty(copyPlant.PlantName) || copyPlant.PlantWaterRequirement <= 0)
            {
                AlertDescription = "Pflanze konnte nicht hinzugefügt werden. \n Pflanzenname und Wasserbedarf sind Pflichtfelder.\n Pflanze konnte nicht hinzugefügt werden.";
                await App.Current!.MainPage!.DisplayAlert("Warning", AlertDescription, "Ok");
                return;
            }
            else
            {
                // Update the plant's room with the selected room
                SelectedPlant.PlantName = copyPlant.PlantName;
                SelectedPlant.PlantNameScientific = copyPlant.PlantNameScientific;
                SelectedPlant.PlantWaterLastTime = copyPlant.PlantWaterLastTime;
                SelectedPlant.PlantWaterRequirement = copyPlant.PlantWaterRequirement;
                SelectedPlant.RoomID = copyPlant.RoomID;

                SelectedPlant.RoomID = SelectedRoom.RoomID;

                try
                {
                    // Initialize the DAL and update plant
                    var dataAccessLayer = new DataAccessLayer();
                    await dataAccessLayer.UpdatePlantsToDB(SelectedPlant);

                    // Navigate back to last overview
                    await App.Current!.MainPage!.Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving plant: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Load all rooms sorted by RoomID
        /// </summary>
        private async void LoadRooms()
        {
            // Create an instance of DAL and load rooms
            var dataAccessLayer = new DataAccessLayer();
            var roomsList = await dataAccessLayer.GetRooms();

            // Add rooms to the list
            foreach (var room in roomsList)
            {
                Rooms.Add(room);
            }

            SelectedRoom = Rooms?.FirstOrDefault(r => r.RoomID == SelectedPlant?.RoomID);
        }

        // Bind this property to your View for editing
        public Plant CopyPlant
        {
            get => copyPlant;
            set => SetProperty(ref copyPlant, value);
        }
    }
}