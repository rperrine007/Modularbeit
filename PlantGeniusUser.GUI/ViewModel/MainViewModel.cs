using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using PlantGeniusUser.GUI.Views;

namespace PlantGeniusUser.GUI.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ICommand NavigateToPlantPageCommand { get; }
        public ICommand NavigateToSettingsPageCommand { get; }

        public MainViewModel()
        {
            NavigateToPlantPageCommand = new Command(NavigateToPlantPage);
            NavigateToSettingsPageCommand = new Command(NavigateToSettingsPage);
        }

        private async void NavigateToPlantPage()
        {
            await Shell.Current.GoToAsync("///PlantPage");
        }


        private async void NavigateToSettingsPage()
        {
            await Shell.Current.GoToAsync("///SettingPage");
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
