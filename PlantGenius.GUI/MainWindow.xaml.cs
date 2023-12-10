using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;


namespace PlantGenius.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //bool to check if the window already changes size.
        private bool isResizing;

        public MainWindow()
        {
            InitializeComponent();

            // initialize boolean
            isResizing = false;
        }

        /// Function: The window size can only be changed proportinally.
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
            if (isResizing)
            {
                return;
            }
            else
            { 
                // If the height is changed set the width accordingly
                if (e.HeightChanged)
                {
                    isResizing = true;
                    //Ratio 0.9
                    this.Width = e.NewSize.Height * (10.0 / 9.0);
                    
                }
                // If the width is changed set height accordingly
                else if (e.WidthChanged)
                {
                    isResizing = true;
                    //Ratio 1.11
                    this.Height = e.NewSize.Width * (9.0 / 10.0);
                }


                isResizing = false;
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

