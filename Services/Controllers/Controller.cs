using Services.AppSettings;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Controllers
{
    public class Controller
    {
        protected ControllersSettings _settings;
        public Controller(ControllersSettings settings) {
            _settings = settings;
        }
        public static List<string> GetAvailableControllers()
        {
            List<string> availableControllers = new List<string>
            {
                "Keyboard"
            };
            var controllers = new[] { new SharpDX.XInput.Controller(UserIndex.One), new SharpDX.XInput.Controller(UserIndex.Two), new SharpDX.XInput.Controller(UserIndex.Three), new SharpDX.XInput.Controller(UserIndex.Four) };
            foreach (var ctr in controllers)
            {
                if (ctr.IsConnected)
                    availableControllers.Add("XBox pad " + ctr.UserIndex.ToString());
            }
            return availableControllers;

        }
        protected virtual void ControllerTask()
        {

            throw new NotImplementedException();
        }


        private void task_wrapper()
        {
            while(_should_work)
            {
                ControllerTask();
            }
        }

        public int Mode {  get; set; }

        protected int _period = 0;
        protected bool _should_work = false;
        Thread? x;
        public void StartController(int period)
        {
            _period = period;
            if(x == null) { 
                x = new Thread(task_wrapper);
                _should_work = true;
                x.Start();
            }
        }
        public Action<float[]>? SendData;
        public void StopController()
        {
            _should_work = false;
       
        }
    }
}
