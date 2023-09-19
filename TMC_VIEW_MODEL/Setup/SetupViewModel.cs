using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMC_API;

namespace TMC_VIEW_MODEL
{
    public partial class SetupViewModel : BaseTMCViewModel
    {

        [ObservableProperty]
        private ObservableCollection<float> _CTRLMatrix = new ObservableCollection<float>(new float[48]);

        [ObservableProperty]
        private float _RollSlider;

        [ObservableProperty]
        private float _PitchSlider;
        [ObservableProperty]
        private float _YawSlider;
        [ObservableProperty]
        private float _VerticalSlider;

        [ObservableProperty]
        private float _LongitudinalSlider;

        [ObservableProperty]
        private float _LateralSlider;



        [RelayCommand]
        private void GetCTRLMatrix()
        { 
        }

        [RelayCommand]
        private void SendCTRLMatrix()
        {

        }

        public override void OnShow()
        {
            
        }

        public override void OnHide()
        {

        }
        public SetupViewModel(TMC_Model TMC_Model) : base(TMC_Model)
        {
            
        }
    }
}
