using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MySqlX.XDevAPI.Common;
using PlantGenius.DAL;
using PlantGenius.DAL.Models;
using PlantGenius.GUI.ViewModel;

namespace PlantGenius.GUI.Views
{
    /// <summary>
    /// Interaction logic for RoomView.xaml
    /// INotifyPropertyChanged interface need to be added to make a two way interaction with the UI interface (data binding context).
    /// </summary>
    public partial class RoomView : Window
    { 

        // Generate a Collection with rooms
        private ObservableCollection<Room> roomList;
        private DataAccessLayer DAL;

        public RoomView()
        {
            InitializeComponent();
            RoomWindowViewModel roomViewModel = new RoomWindowViewModel();
            DAL = new DataAccessLayer();

            //Set Datacontext for binding in WPF
            this.DataContext = roomViewModel;

            //Set sub-datacontext
            roomList = roomViewModel.roomList;
            ListBox_RoomList.DataContext = roomList;
            StackPanel_chosenRoom.DataContext = roomList;
        }

        /// <summary>
        /// Prevents to add non int values to a textfield.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

                TextBox? textBox = sender as TextBox;
            // Allow "-" only if it's the first character, allow digits
            if (textBox != null)
            {
                if (e.Text == "-" && textBox.Text.Length == 0 && !textBox.Text.Contains("-"))
                {
                    // Allow input
                    e.Handled = false;
                }
                else
                {
                    // Allow digits only
                    foreach (char c in e.Text)
                    {
                        if (!char.IsDigit(c))
                        {
                            // Block input
                            e.Handled = true;
                            break;
                        }
                    }
                }
            }
            
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = string.Empty;
            // Oder, wenn Sie einen bestimmten Standardtext haben:
            // textBox.Text = "Neuer Standardtext";
        }

    }
}
