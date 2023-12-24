using System;
using System.ComponentModel;
using System.Windows.Input;
using PlantGenius.DAL;
using PlantGenius.DAL.Models;

namespace PlantGeniusUserApp.GUI.ViewModel
{
    public class PlantEditViewModel : INotifyPropertyChanged
    {
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
        }

        private async void SavePlant()
        {
            if (SelectedPlant != null)
            {
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


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
