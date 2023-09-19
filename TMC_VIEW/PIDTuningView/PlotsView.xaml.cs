using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace TMC_VIEW.PIDTuningView
{
    /// <summary>
    /// Interaction logic for PlotsView.xaml
    /// </summary>
    public partial class PlotsView : UserControl
    {
        public PlotsView()
        {
            InitializeComponent();
        }

        private void chk_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chbox = (CheckBox)sender;
            if (chbox == null)
                return;
            if(Container == null)
                return;



            if (chk_Roll.IsChecked == true)
                row_roll.Height = new GridLength(1, GridUnitType.Star);
            else
                row_roll.Height = new GridLength(1.0, GridUnitType.Auto);

            if (chk_Pitch.IsChecked == true)
                row_pitch.Height = new GridLength(1, GridUnitType.Star);
            else
                row_pitch.Height = new GridLength(1.0, GridUnitType.Auto);

            if (chk_Yaw.IsChecked == true)
                row_yaw.Height = new GridLength(1, GridUnitType.Star);
            else
                row_yaw.Height = new GridLength(1.0, GridUnitType.Auto);

            if (chk_Vertical.IsChecked == true)
                row_vspeed.Height = new GridLength(1, GridUnitType.Star);
            else
                row_vspeed.Height = new GridLength(1.0, GridUnitType.Auto);





        }
    }
}
