using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantGenius.DAL;
using PlantGenius.DAL.Models;

namespace PlantGeniusUserApp.GUI.ViewModel
{
    public partial class PlantEditViewModel : ObservableObject
    {
        public ObservableCollection<Room> Rooms { get; private set; }

        [ObservableProperty]
        private Room selectedRoom;

        [ObservableProperty]
        private Plant selectedPlant;

        public PlantEditViewModel(Plant selectedPlant)
        {
            SelectedPlant = selectedPlant;
            Rooms = new ObservableCollection<Room>();
            LoadRooms();
        }

        private bool CanSavePlant(object obj)
        {
            return true;
        }

        [RelayCommand(CanExecute = nameof(CanSavePlant))]
        private async Task SavePlant(object obj)
        {
            if (SelectedPlant != null)
            {
                // Update the plant's room with the selected room
                SelectedPlant.RoomID = SelectedRoom.RoomID;

                try
                {
                    var dataAccessLayer = new DataAccessLayer();
                    await dataAccessLayer.UpdatePlantsToDB(SelectedPlant);
                    await App.Current.MainPage.Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving plant: {ex.Message}");
                }
            }
        }


        private async void LoadRooms()
        {
            var dataAccessLayer = new DataAccessLayer();
            var roomsList = await dataAccessLayer.GetRooms();
            foreach (var room in roomsList)
            {
                Rooms.Add(room);
            }

            SelectedRoom = Rooms.FirstOrDefault(r => r.RoomID == SelectedPlant.RoomID);
        }
    }
}
