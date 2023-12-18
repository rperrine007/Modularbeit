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

        private void NavigateToPlantPage()
        {
            Shell.Current.GoToAsync(nameof(PlantPage));

        }

        private void NavigateToSettingsPage()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
