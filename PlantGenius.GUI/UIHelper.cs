using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlantGenius.GUI
{
    public class UIHelper
    {
        /// <summary>
        /// In Method keeps window position and size if going to another view
        /// </summary>
        /// <param name="currentWindow"></param>
        /// <param name="newWindow"></param>
        public static void SwitchWindowKeepSizePosition(Window currentWindow, Window newWindow)
        {
            // Save the position of the window to keep size and position
            newWindow.Left = currentWindow.Left;
            newWindow.Top = currentWindow.Top;
            newWindow.Width = currentWindow.Width;
            newWindow.Height = currentWindow.Height;

            // Open the new window and close old one
            newWindow.Show();
            currentWindow.Close();
        }
    }
}
