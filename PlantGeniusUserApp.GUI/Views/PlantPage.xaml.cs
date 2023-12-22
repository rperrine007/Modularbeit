using System;
using Microsoft.Maui.Controls;
using PlantGeniusUserApp.GUI.ViewModel;

namespace PlantGeniusUserApp.GUI.Views
{
    public partial class PlantPage : ContentPage
    {
        public PlantPage()
        {
            InitializeComponent();
            BindingContext = new PlantsViewModel();
        }

    }
}
