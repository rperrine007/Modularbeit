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
        
        // TODO close mainwindow
        private void ShowWindow(object obj)
        {
            RoomView roomViewWin = new RoomView();
            roomViewWin.Show();


            /*
            // Save the position of the window to keep size and position
            roomViewWin.Left = currentWindow.Left;
            roomViewWin.Top = currentWindow.Top;
            roomViewWin.Width = currentWindow.Width;
            roomViewWin.Height = currentWindow.Height;

            // Open the new window and close old one
            roomViewWin.Show();
            //currentWindow.Close();*/
        }

    }
}
