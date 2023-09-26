using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace TMC_VIEW.TopPanel
{
    /// <summary>
    /// Interaction logic for TopPanel.xaml
    /// </summary>
    public partial class TopPanel : UserControl
    {
        public TopPanel()
        {
            InitializeComponent();
        }
        private void COM_DropDownOpened(object sender, EventArgs e)
        {
            List<string> src = new List<string>();
            var t = SerialPort.GetPortNames();
            foreach (var name in t)
            {
                if (!src.Contains(name))
                {
                    src.Add(name);
                }
            }
            COM.ItemsSource = src;// SerialPort.GetPortNames();
        }

        private void IPBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^-?[0-9]*(?:\.[0-9]*)?$");
            e.Handled = !regex.IsMatch(e.Text);

        }

        private void BaudRateBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^?[0-9]");
            e.Handled = !regex.IsMatch(e.Text);
        }


    }
}
