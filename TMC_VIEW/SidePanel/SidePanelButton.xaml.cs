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

namespace TMC_VIEW.SidePanel
{
    /// <summary>
    /// Interaction logic for SidePanelButton.xaml
    /// </summary>
    public partial class SidePanelButton : RadioButton
    {
        public SidePanelButton()
        {
            InitializeComponent();
        }
        //public ICommand Command
        //{
        //    get { return (ICommand)GetValue(CommandProperty); }
        //    set { SetValue(CommandProperty, value); }
        //}
        //public static readonly DependencyProperty CommandProperty =
        //    DependencyProperty.Register("Command",
        //        typeof(ICommand), typeof(SidePanelButton));

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image",
                typeof(ImageSource), typeof(SidePanelButton));


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text",
                typeof(string), typeof(SidePanelButton));

        //public string CommandParamater
        //{
        //    get { return (string)GetValue(CommandParamaterProperty); }
        //    set { SetValue(CommandParamaterProperty, value); }
        //}
        //public static readonly DependencyProperty CommandParamaterProperty =
        //    DependencyProperty.Register("CommandParamater",
        //        typeof(string), typeof(SidePanelButton));
    }
}


