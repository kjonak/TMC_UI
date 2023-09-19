using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Services.Dialogs
{
    public class DialogService
    {
        public static void OpenDialog(DialogBaseViewModel vm)
        {
            Window win = new DialogWindow();
            
            win.DataContext = vm;
            win.Owner = Application.Current.MainWindow;
          
            win.ShowDialog();
        }
    }
}
