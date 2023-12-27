using PlantGeniusUserApp.GUI.ViewModel;

namespace PlantGeniusUserApp.GUI.Views;

public partial class RoomPage : ContentPage
{
	public RoomPage()
	{
		InitializeComponent();
        BindingContext = new RoomViewModel();
    }
}