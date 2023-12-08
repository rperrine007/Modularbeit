using System;
using System.Windows;


namespace PlantGenius.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();           
        }

        /// Function: The window size can only be changed proportinally.
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // If the height is changed set the width accordingly
            if (e.HeightChanged)
            {
                //Ratio 0.9
                double newWidth = e.NewSize.Height * (10.0 / 9.0); 
                this.Width = newWidth;
            }
            // If the width is changed set height accordingly
            else if (e.WidthChanged)
            {
                //Ratio 1.11
                double newHeight = e.NewSize.Width * (9.0 / 10.0); 
                this.Height = newHeight;
            }
        }

        /// <summary>
        /// When the button "Räume bearbeiten" is clicked, change to the Room window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeToRoomView(object sender, RoutedEventArgs e)
        {
            RoomView roomView = new RoomView();
            UIHelper.SwitchWindowKeepSizePosition(this, roomView);
        }
    }
}

