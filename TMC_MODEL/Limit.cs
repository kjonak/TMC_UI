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

        public byte[] Serialize()
        {
            byte[] ret = new byte[sizeof(float) * 2];

            var p = BitConverter.GetBytes(Min);
            var i = BitConverter.GetBytes(Max);


            Buffer.BlockCopy(p, 0, ret, 0, sizeof(float));
            Buffer.BlockCopy(i, 0, ret, 4, sizeof(float));

            return ret;
        }
    }
}
