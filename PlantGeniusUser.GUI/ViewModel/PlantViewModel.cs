using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using PlantGenius.DAL.Models;

namespace PlantGeniusUser.GUI.ViewModel 
{
    public class PlantsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Plant> Plants { get; set; }
        public ICommand EditCommand { get; private set; }
        public ICommand WaterCommand { get; private set; }

        public PlantsViewModel()
        {
            // Initialize ObservableCollection
            Plants = new ObservableCollection<Plant>();
            EditCommand = new Command<Plant>(EditPlant);
            WaterCommand = new Command<Plant>(WaterPlant);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void EditPlant(Plant plant)
        {
            // 
        }

        private void WaterPlant(Plant plant)
        {
            // 
        }

    }
}

