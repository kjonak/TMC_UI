using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Services.Dialogs
{
    public partial class DialogBaseViewModel : ObservableObject
    {
        [RelayCommand]
        private void CloseWindow(Window window)
        {
            window.Close();
        }
    }
}
