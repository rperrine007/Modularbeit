namespace PlantGeniusUserApp.GUI.Views;

public partial class PlantPageEdit : ContentPage
{
	public PlantPageEdit()
	{
		InitializeComponent();
	}

    // To check if input has just numbers
    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!(sender is Entry entry))
            return;

        entry.Text = new string(entry.Text.Where(char.IsDigit).ToArray());
    }

}