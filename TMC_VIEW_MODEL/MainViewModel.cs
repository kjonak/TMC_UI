using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Services.AppSettings;
using Services.Dialogs;
using Services.Dialogs.SettingsDialog.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMC_API;
using TMC_VIEW_MODEL;


namespace TMC_VIEW_MODEL
{
    

    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private TMC_Model _TMC_Model;


        [ObservableProperty]
        private SetupViewModel _SetupVM;

        [ObservableProperty]
        private PIDTuningViewModel _PIDTuningVM;

        [ObservableProperty]
        private BaseTMCViewModel _ControlVM;

        [ObservableProperty]
        private BaseTMCViewModel _SelectedVM;

        [RelayCommand]
        private void ChangeSelectedVM(string arg)
        {
            SelectedVM.OnHide();
            //string arg = "PID";
            switch(arg)
            {
                case "Setup":
                    SelectedVM = SetupVM; break;
                case "PID":
                    SelectedVM = PIDTuningVM; break;
                case "Control":
                    SelectedVM  = ControlVM; break;
            }
            SelectedVM.OnShow();
        }


        [RelayCommand]
        private void OpenSettings()
        {
           
            DialogBaseViewModel vm = new SettingsDialogViewModel(AppSettings);
            DialogService.OpenDialog(vm);
        }
        [ObservableProperty]
        AppSettings _appSettings;
        public MainViewModel(AppSettings appSettings)
        {
            _appSettings = appSettings;
            
            TMC_Model = new TMC_Model();

            SetupVM = new SetupViewModel(TMC_Model);
            PIDTuningVM = new PIDTuningViewModel(TMC_Model);
            ControlVM = new ControlViewModel(TMC_Model);

            SelectedVM = PIDTuningVM;

        }

    }
}
