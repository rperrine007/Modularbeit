using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using PlantGeniusUser.GUI.Views;

namespace PlantGeniusUser.GUI.ViewModel
{
    public class SettingViewModel : INotifyPropertyChanged
    {
        public ICommand NavigateToPlantPageCommand { get; }
        public ICommand NavigateToSettingsPageCommand { get; }

        public SettingViewModel()
        {
            NavigateToPlantPageCommand = new Command(NavigateToPlantPage);
            NavigateToSettingsPageCommand = new Command(NavigateToSettingsPage);
        }

        private async void NavigateToPlantPage()
        {
            try
            {
                await Shell.Current.GoToAsync("///PlantPage");
            }
            catch (Exception ex)
            {
                // Log the exception or display an alert
                Console.WriteLine(ex.Message);
            }
        }


        private void NavigateToSettingsPage()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
