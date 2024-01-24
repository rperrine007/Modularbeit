using PlantGenius.GUI.ViewModel;
using System;
using System.Threading.Tasks;
using System.Windows;


namespace PlantGenius.GUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            MainWindowViewModel mainViewModel = new MainWindowViewModel();
            this.DataContext = mainViewModel;
        }
    }
}

