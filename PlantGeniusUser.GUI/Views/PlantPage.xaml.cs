using PlantGeniusUser.GUI.ViewModel;

namespace PlantGeniusUser.GUI.Views
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