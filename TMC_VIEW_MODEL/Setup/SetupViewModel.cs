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

        [ObservableProperty]
        double test = 123;
        [RelayCommand]
        void SendPID()
        {
            TMC_Model.SendPID?.Invoke();
            Console.WriteLine("Sending PID");
        }

        [RelayCommand]
        void GetPID()
        {
            TMC_Model.RequestPID?.Invoke();
            Console.WriteLine("PID requsted!");
        }

        [RelayCommand]
        void SendCTRLMatrix()
        {
            TMC_Model.SendControlMatrix?.Invoke();
            Console.WriteLine("Sending CL matrix");
        }
        [RelayCommand]
        void GetCTRLMatrix()
        {
            TMC_Model.RequestControlMatrix?.Invoke();
            Console.WriteLine("Control matrix requsted!");

        }

        [RelayCommand]
        void SendLimits()
        {
            TMC_Model.SendLimits?.Invoke();
            Console.WriteLine("Sending limits");
        }
        [RelayCommand]
        void GetLimits()
        {
            TMC_Model.RequestLimits?.Invoke();
            Console.WriteLine("Limits requsted!");
        }

        [RelayCommand]
        void SendFrequency()
        {
            TMC_Model.SendTaskFrequency?.Invoke();
            Console.WriteLine("Sending frequency");
  
        }
        [RelayCommand]
        void GetFrequency()
        {
            TMC_Model.RequesTaskFrequency?.Invoke();
            Console.WriteLine("Frequency requsted!");
        }



        public override void OnShow()
        {

        }

        public override void OnHide()
        {

        }
        public SetupViewModel(TMC_Model TMC_Model) : base(TMC_Model)
        {
            var x = new Thread(() => { TMC_Model.PIDRoll.KI += 1; Thread.Sleep(500); });
            x.Start();
        }

    }
}
