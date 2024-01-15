using CommunityToolkit.Mvvm.ComponentModel;
using PlantGenius.DAL.Models;
using PlantGenius.DAL;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using CommunityToolkit.Mvvm.Input;


namespace PlantGeniusUserApp.GUI.ViewModel
{
    /// <summary>
    /// Logic to add a new Plant to the Database
    /// </summary>
    public partial class AddPlantViewModel : ObservableObject
    {
        //Starting an instance for database
        private DataAccessLayer DAL;

        //datavariables
        [ObservableProperty]
        private Room selectedRoom;

        //Add the current time so it does not need to be set if new plant is added
        [ObservableProperty]
        private DateTime currentDateTime;

        //Properties
        public ObservableHashSet<string> ExistingPlantNames { get; set; }
        [ObservableProperty]
        private Plant selectedPlant;
        public String AlertTitle { get; set; }
        public String AlertDescription { get; set; }
        public ObservableCollection<Room> Rooms { get; private set; }



        public AddPlantViewModel()
        {
            // Update the current date and time
            CurrentDateTime = DateTime.Now;

            // Initialize the DAL
            DAL = new DataAccessLayer();
            
            // Initalize the Collections
            ExistingPlantNames = new ObservableHashSet<string>();
            selectedPlant = new Plant();
            Rooms = new ObservableCollection<Room>();

            // Now get the plants and rooms from database
            GetPlantIDsFromDB();
            LoadRooms();
        }

        // Async get plant names from the database
        private async void GetPlantIDsFromDB()
        {
            ExistingPlantNames.Clear();
            var plants = await DAL.LoadPlantsFromDB();
            foreach (var plant in plants)
            {
                ExistingPlantNames.Add(plant.PlantName);
            }
        }

        // Async get room names from the database
        private async void LoadRooms()
        {
            var dataAccessLayer = new DataAccessLayer();
            var roomsList = await dataAccessLayer.GetRooms();
            foreach (var room in roomsList)
            {
                Rooms.Add(room);
            }

            // Add the Room to the plant
            SelectedRoom = Rooms.FirstOrDefault(r => r.RoomID == SelectedPlant.RoomID);
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

            //checks if a Plant is not null
            if (SelectedPlant == null || string.IsNullOrEmpty(SelectedPlant.PlantName) || SelectedRoom == null || SelectedPlant.PlantWaterRequirement == 0)
            {
                AlertDescription = "Pflanze konnte nicht hinzugefügt werden. \n Pflanzenname, Raum, Wasserbedarf(Tage) und letzted Giessdatum sind Pflichtfelder.\n Pflanze konnte nicht hinzugefügt werden.";
                await App.Current.MainPage.DisplayAlert("Warning", AlertDescription, "Ok");
                return;
            }
            //check if a Plant with the given name already exists.
            else if (ExistingPlantNames.Contains(SelectedPlant.PlantName))
            {
                AlertDescription = "Pflanze konnte nicht hinzugefügt werden da es bereits eine Pflanze mit dem angegeben Namen gibt. \n Bitte wählen Sie einen anderen Namen.";
                await App.Current.MainPage.DisplayAlert("Warning", AlertDescription, "Ok");
                return;
            }

            // Create a new Plant object from the input
            Plant newPlant = new Plant()
            {
                PlantName = SelectedPlant.PlantName,
                PlantNameScientific = SelectedPlant.PlantNameScientific ?? string.Empty,
                RoomID = SelectedRoom.RoomID,
                Room = SelectedRoom,
                PlantWaterRequirement = SelectedPlant.PlantWaterRequirement,
                PlantWaterLastTime = SelectedPlant.PlantWaterLastTime,
            };

            //add Plant to DB
            await DAL.AddPlantToDB(newPlant);

            //add to existing names Set so we can make sure not two plants with the same name exist.
            ExistingPlantNames.Add(newPlant.PlantName);

            try
            {
                await App.Current.MainPage.Navigation.PopAsync();
                return;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Warning", $"Error saving plant: {ex.Message}", "Ok");
            }
        }
    }
}
