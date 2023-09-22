using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Services.Dialogs.SettingsDialog.ViewModel
{
    public partial class SettingsDialogViewModel : DialogBaseViewModel
    {
        [ObservableProperty]
        AppSettings.AppSettings _appSettings;

    
        [RelayCommand]
        void Save()
        {
            Console.WriteLine("Saving settings to AppSettings.xaml");
            AppSettings.Save("AppSettings.xaml");
        }
        public SettingsDialogViewModel(AppSettings.AppSettings appSettings) 
        {
            AppSettings = appSettings;
            
        }
 
    }
}
