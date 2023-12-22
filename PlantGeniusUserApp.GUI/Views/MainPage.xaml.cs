using PlantGeniusUserApp.GUI.ViewModel;
namespace PlantGeniusUserApp.GUI.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }
}