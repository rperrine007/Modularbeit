using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

        private RoomWindowViewModel roomViewModel;

        public RoomView()
        {
            InitializeComponent();
            roomViewModel = new RoomWindowViewModel();

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

        private void SelectionChangedEvent(object sender, SelectionChangedEventArgs e)
        {
            ListBox? listBox = null;
            listBox = sender as ListBox;
            if (e.RemovedItems.Count != 0 && listBox.Items.Count > 0)
            { 
                // TODO: good idea?
                var result = MessageBox.Show("Achtung: Änderungen wurden nicht in die Datenbank gespeichert!");
            }                

        }


    }
}
