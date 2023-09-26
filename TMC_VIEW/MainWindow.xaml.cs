using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TMC_VIEW_MODEL;

namespace TMC_VIEW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DoOnStartup();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            WindowControl.RefreshMaximizeRestoreButton();
        }
        private void DoOnStartup()
        {
            WindowControl.RefreshMaximizeRestoreButton();
        }



        private void Scroller_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if(Scroller.IsMouseOver == false)
                Scroller.ScrollToEnd();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var vm = (MainViewModel)this.DataContext;
            vm.OnClose();
        }
    }
}
