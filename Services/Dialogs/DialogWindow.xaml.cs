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

namespace Services.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        private bool isClosing = false;
        public DialogWindow()
        {
            InitializeComponent();
        }
        private void DialogWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isClosing = true;
        }
        private void Window_Deactivated(object sender, EventArgs e)
        {
            //if (!isClosing)
            //    Close();

        }
    }
}
