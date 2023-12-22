using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PlantGeniusUserApp.GUI.Views;

namespace PlantGeniusUserApp.GUI.ViewModel
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
