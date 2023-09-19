using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMC_API
{
    public partial class Limit : ObservableObject
    {

        [ObservableProperty]
        private float _Min;
        [ObservableProperty]
        private float _Max;
    }
}
