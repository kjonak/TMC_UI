using Services.AppSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Controllers
{
    internal class XboxPadController : Controller
    {
        public XboxPadController(ControllersSettings settings) : base(settings)
        {
        }

        protected override void ControllerTask()
        {
            base.ControllerTask();
        }
    }
}
