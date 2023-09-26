using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMC_API
{
    public partial class PID : ObservableObject
    {
        [ObservableProperty]
        float _kP;
        [ObservableProperty]
        float _kI;
        [ObservableProperty]
        float _kD;
        [ObservableProperty]
        float _kL;
        [ObservableProperty]
        float _IMax;
        public byte[] Serialize()
        {
            byte[] ret = new byte[sizeof(float) * 5];

            var p = BitConverter.GetBytes(KP);
            var i = BitConverter.GetBytes(KI);
            var d = BitConverter.GetBytes(KD);
            var imax = BitConverter.GetBytes(IMax);
            var l = BitConverter.GetBytes(KL);

            Buffer.BlockCopy(p, 0, ret, 0, sizeof(float));
            Buffer.BlockCopy(i, 0, ret, 4, sizeof(float));
            Buffer.BlockCopy(d, 0, ret, 8, sizeof(float));
            Buffer.BlockCopy(imax, 0, ret, 12, sizeof(float));
            Buffer.BlockCopy(l, 0, ret, 16, sizeof(float));
            return ret;
        }

        public void Decode(byte[] data)
        {
            if (data.Length != 5 * sizeof(float))
                return;
            KP = BitConverter.ToSingle(data, 0 * 4);
            KI = BitConverter.ToSingle(data, 1 * 4);
            KD = BitConverter.ToSingle(data, 2 * 4);
            IMax = BitConverter.ToSingle(data, 3 * 4);
            KL = BitConverter.ToSingle(data, 4 * 4);
        }
    }
}
