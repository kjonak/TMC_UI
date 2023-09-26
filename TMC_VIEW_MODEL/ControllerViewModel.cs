using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Services.AppSettings;
using Services.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TMC_API;

namespace TMC_VIEW_MODEL
{
    public partial class ControllerViewModel : BaseTMCViewModel
    {
        AppSettings appSettings;

        [ObservableProperty]
        bool _armed = false;

  

        public string SelectedMode { get { return ""; } 
            set 
            { 
                if (value.Contains("Stable")) controller.Mode = 0; 
                else if (value.Contains("Rate")) controller.Mode = 1;
                else if (value.Contains("Service")) controller.Mode = 254;
            } }

        [ObservableProperty]
        string _selectedController = "Keyboard";

        private Controller controller;

        [RelayCommand]
        void Arm()
        {
            Armed = true;
            if (SelectedController == "Keyboard")
                controller = new KeyboardController(appSettings.ControllersSettings);
            controller.SendData = TMC_Model.SendSticks;
            controller.StartController((int)appSettings.ControllerInterval);
        }

        [RelayCommand]
        void Disarm()
        {
            controller.StopController();
            Armed = false;
        }
        public ControllerViewModel(TMC_Model TMC_Model, AppSettings settings) : base(TMC_Model)
        {
            appSettings = settings;
            controller = new Controller(appSettings.ControllersSettings);
        }
    }
}
