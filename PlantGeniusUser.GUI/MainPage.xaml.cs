using System;
using Microsoft.Maui.Controls;
using PlantGeniusUser.GUI.ViewModel;

namespace PlantGeniusUser.GUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new MainViewModel();
        }
    }
}
