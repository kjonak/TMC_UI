using Services.AppSettings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TMC_VIEW;
using TMC_VIEW_MODEL;

namespace TMC_UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            
            AppSettings? appSettings = AppSettings.Read("test.xaml");
            if(appSettings == null)
            {
                appSettings = new AppSettings();
                appSettings.ControllersSettings.LoadDefaultValues();
            }
            MainWindow window = new MainWindow();
            MainViewModel MainVM= new MainViewModel(appSettings);

            window.DataContext = MainVM;
            this.MainWindow = window;
            window.Show();
        }


    }
}
