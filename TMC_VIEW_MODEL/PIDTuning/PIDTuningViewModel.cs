using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMC_API;
using TMC_VIEW_MODEL.PIDTuning;

namespace TMC_VIEW_MODEL
{
    public partial class PIDTuningViewModel : BaseTMCViewModel
    {
        [RelayCommand]
        private void PIDSaveBtn()
        {

        }

        [RelayCommand]
        private void PIDReloadBtn()
        {

        }
        [ObservableProperty]
        BaseTMCViewModel _PlotsVM;

        public override void OnShow()
        {
            PlotsVM.OnShow();
        }

        public override void OnHide()
        {
            PlotsVM.OnHide();
        }
        public PIDTuningViewModel(TMC_Model TMC_Model) : base(TMC_Model)
        {
            PlotsVM = new PlotsViewModel(TMC_Model);
            this.OnShow();
        }
    }
}
