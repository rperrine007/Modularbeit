using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using PlantGenius.GUI;
using PlantGenius.DAL.Models;
using PlantGenius.GUI.Commands;
using PlantGenius.GUI.Views;
using System.Windows;

namespace PlantGenius.GUI.ViewModel
{
        class MainWindowViewModel
    {

        public ICommand ShowWindowCommand { get; set; }

        public MainWindowViewModel() 
        {
            ShowWindowCommand = new RelayCommand(ShowWindow, CanShowWindow);
        }

        private bool CanShowWindow(object obj)
        {
            //We always want to execute the command.
            return true;
        }
        
        private void ShowWindow(object obj)
        {
            RoomView roomViewWin = new RoomView();
            roomViewWin.Show();

            //initialize variabel with defined command parameter and cast it as type Window
            var mainWindow = obj as Window;

            // Save the position of the window to keep size and position
            roomViewWin.Left = mainWindow.Left;
            roomViewWin.Top = mainWindow.Top;
            roomViewWin.Width = mainWindow.Width;
            roomViewWin.Height = mainWindow.Height;

            // Open the new window and close old one
            roomViewWin.Show();
            mainWindow.Close();
        }

    }
}
