using Services.AppSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Services.Controllers
{
    public class KeyboardController : Controller
    {

        public KeyboardController(ControllersSettings settings) : base(settings)
        {

        }

        float[] data_to_send = new float[8];
        DateTime lastSend = DateTime.Now;
        protected override void ControllerTask()
        {
            
            CalculateOutput();
            if ((DateTime.Now - lastSend) > TimeSpan.FromMilliseconds(_period))
            {
                SendData?.Invoke(data_to_send);
                lastSend = DateTime.Now;
            }
            Thread.Sleep(5);
        }

        void CalculateOutput()
        {
            var keys = _settings.KeyboardAssignment;
            int cnt = 0;
            int index = 0;
            data_to_send[0] = 1; //armed
            data_to_send[1] = Mode; //armed
            bool keyDown = false;
            bool[] index_presed = new bool[8];
            foreach (var key in keys)
            {
                index = cnt++ / 2 + 2;
                
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    keyDown = Keyboard.IsKeyDown(key);

                }
                ));
                if (keyDown)
                {
                    index_presed[index] = true;
                    if (cnt % 2 == 0)
                    {
                        data_to_send[index] += 0.01f;
                    }
                    else
                    {
                        data_to_send[index] -= 0.01f;
                    }
                }
            }
            for(int i =2; i<8;i++)
            {
                if (!index_presed[i])
                    data_to_send[i] *= 0.95f;
                if (data_to_send[i] > 1.0f)
                    data_to_send[i] = 1.0f;
                if (data_to_send[i] < -1.0f)
                    data_to_send[i] = -1.0f;
            }

        }

        //DateTime lastSend = DateTime.Now;
        //private void task_wrapper()
        //{
        //    while (_should_work)
        //    {
        //        ControllerTask();
        //        if ((DateTime.Now - lastSend) > TimeSpan.FromMilliseconds(_period))
        //        {
        //            SendData?.Invoke(DataToSend);
        //            lastSend = DateTime.Now;
        //        }
        //        Thread.Sleep(5);
        //    }
        //}
    }
}
