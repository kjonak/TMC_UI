using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TMC_VIEW
{
    /// <summary>
    /// Interaction logic for ControllerView.xaml
    /// </summary>
    public partial class ControllerView : UserControl
    {
        public ControllerView()
        {
            InitializeComponent();
            cb.ItemsSource = Services.Controllers.Controller.GetAvailableControllers();
        }

        private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            var src = Services.Controllers.Controller.GetAvailableControllers();
            cb.ItemsSource = src;
        }
    }
}
