using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using PlantGenius.GUI;
using PlantGenius.DAL.Models;
using PlantGenius.GUI.Views;
using System.Windows;
using CommunityToolkit.Mvvm.Input;

namespace PlantGenius.GUI.ViewModel
{
        public partial class MainWindowViewModel : ObservableObject
        {
        public MainWindowViewModel() 
            {
            }

        private bool CanShowWindow(object obj)
        {
            //We always want to execute the command.
            return true;
        }

        [RelayCommand(CanExecute = nameof(CanShowWindow))]
        private void ShowWindow(object obj)
        {
            RoomView roomViewWin = new RoomView();

            //initialize variabel with defined command parameter and cast it as type Window
            var mainWindow = obj as Window;

            try
            {

                // Save the position of the window to keep size and position
                if (mainWindow != null)
                {
                    roomViewWin.Left = mainWindow.Left;
                    roomViewWin.Top = mainWindow.Top;
                    roomViewWin.Width = mainWindow.Width;
                    roomViewWin.Height = mainWindow.Height;

                    // Open the new window and close old one
                    roomViewWin.Show();
                    mainWindow.Close();
                }
            }
            catch(ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
