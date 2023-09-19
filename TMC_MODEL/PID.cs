using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMC_API
{
    public partial class PID : ObservableObject
    {
        [ObservableProperty]
        private float _kP;
        [ObservableProperty]
        private float _kI;
        [ObservableProperty]
        private float _kD;
        [ObservableProperty]
        private float _kL;
        [ObservableProperty]
        private float _IMax;
    }
}
