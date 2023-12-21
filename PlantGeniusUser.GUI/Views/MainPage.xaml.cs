using System;
using Microsoft.Maui.Controls;
using PlantGeniusUser.GUI.ViewModel;

namespace PlantGeniusUser.GUI.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }
}
