using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace PlantGeniusUser.GUI.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _MainProperty;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            // Initialize commands
            MainCommand = new Command(OnMainCommandExecuted);
        }

        public string MainProperty
        {
            get => _MainProperty;
            set
            {
                if (_MainProperty != value)
                {
                    _MainProperty = value;
                    OnPropertyChanged(nameof(MainProperty));
                }
            }
        }

        public ICommand MainCommand { get; }

        private void OnMainCommandExecuted()
        {
            // Command execution logic here
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
