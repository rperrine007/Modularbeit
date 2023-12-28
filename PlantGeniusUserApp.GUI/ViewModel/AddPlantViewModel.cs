using CommunityToolkit.Mvvm.ComponentModel;
using PlantGenius.DAL.Models;
using PlantGenius.DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using Org.BouncyCastle.Tls;



namespace PlantGeniusUserApp.GUI.ViewModel
{
    public partial class AddPlantViewModel : ObservableObject
    {
        //DB
        private DataAccessLayer DAL;

        //datavariables
        [ObservableProperty]
        private Room selectedRoom;

        //Properties
        public ObservableHashSet<string> ExistingPlantNames { get; set; }
        [ObservableProperty]
        private Plant selectedPlant;
        public String AlertTitle { get; set; }
        public String AlertDescription { get; set; }
        public ObservableCollection<Room> Rooms { get; private set; }



        public AddPlantViewModel()
        {
            // Initialize 
            DAL = new DataAccessLayer();
            ExistingPlantNames = new ObservableHashSet<string>();
            selectedPlant = new Plant();
            Rooms = new ObservableCollection<Room>();

            // Now get the plant-IDs from database
            GetPlantIDsFromDB();

            LoadRooms();
        }

        private async void GetPlantIDsFromDB()
        {
            ExistingPlantNames.Clear();
            var plants = await DAL.LoadPlantsFromDB();
            foreach (var plant in plants)
            {
                ExistingPlantNames.Add(plant.PlantName);
            }
        }

        private async void LoadRooms()
        {
            var roomsList = await DAL.GetRooms();
            foreach (var room in roomsList)
            {
                Rooms.Add(room);
            }

            SelectedRoom = Rooms.FirstOrDefault(r => r.RoomID == SelectedPlant.PlantRoom);
        }

        private bool CanAddPlant(object obj)
        {
            return true;
        }

        /// <summary>
        /// Adds a new room to the roomList and the DB.
        /// </summary>
        /// <param name="obj"></param>
        [RelayCommand(CanExecute = nameof(CanAddPlant))]
        private async Task AddPlant(object obj)
        {

            //checks if a Room is not null
            if ((SelectedPlant == null) || (SelectedPlant.PlantName == string.Empty) || (SelectedRoom == null) || (SelectedPlant.PlantWaterRequirement == 0) || (SelectedPlant.PlantWaterLastTime == null) )
            {
                AlertTitle = "Fehler";
                AlertDescription = "Pflanz konnte nicht hinzugefügt werden. \nRaumname, RaumID sind Pflichtfelder";
                return;
            }
            //check if a Room with the given name already exists.
            else if (ExistingPlantNames.Contains(SelectedPlant.PlantName))
            {
                AlertTitle = "Fehler";
                AlertDescription = "Raum konnte nicht hinzugefügt werden. \nEs gibt bereits einen Raum mit dem angegeben Namen. Bitte ändere den Namen.";
                return;
            }

            // Create a new Room object from the input
            Plant newPlant = new Plant()
            {
                PlantName = SelectedPlant.PlantName,
                PlantNameScientific = SelectedPlant.PlantNameScientific == null ? string.Empty : SelectedPlant.PlantNameScientific,
                PlantRoom = SelectedRoom.RoomID,
                Room = SelectedRoom,
                PlantWaterRequirement = SelectedPlant.PlantWaterRequirement,
                PlantWaterLastTime = SelectedPlant.PlantWaterLastTime,
            };

            //add Room to DB
            await DAL.AddPlantToDB(newPlant);

            //add to exsiting names Set so we can make sure not two rooms with the same name exist.
            ExistingPlantNames.Add(newPlant.PlantName);

            try
            {
                await App.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving plant: {ex.Message}");
            }
        }

    }


}
