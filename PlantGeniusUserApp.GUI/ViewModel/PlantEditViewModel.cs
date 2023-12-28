using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using PlantGenius.DAL;
using PlantGenius.DAL.Models;

namespace PlantGeniusUserApp.GUI.ViewModel
{
    public class PlantEditViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Room> Rooms { get; private set; }
        private Room _selectedRoom;
        public Room SelectedRoom
        {
            get { return _selectedRoom; }
            set
            {
                if (_selectedRoom != value)
                {
                    _selectedRoom = value;
                    OnPropertyChanged(nameof(SelectedRoom));
                }
                
            }
        }

        private Plant _selectedPlant;
        public Plant SelectedPlant

        {
            get { return _selectedPlant; }
            set
            {
                if (_selectedPlant != value)
                {
                    _selectedPlant = value;
                    OnPropertyChanged(nameof(SelectedPlant));
                }
            }
        }

        public ICommand SaveCommand { get; private set; }

        public PlantEditViewModel(Plant selectedPlant)
        {
            SelectedPlant = selectedPlant;
            SaveCommand = new Command(SavePlant);

            Rooms = new ObservableCollection<Room>();
            LoadRooms();
        }

        private async void SavePlant()
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


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
