namespace PlantGeniusUserApp.GUI.Views;
using PlantGeniusUserApp.GUI.ViewModel;

public partial class AddPlantPage:ContentPage
{
	public AddPlantPage()
	{
		InitializeComponent();
        BindingContext = new AddPlantViewModel();
    }
}