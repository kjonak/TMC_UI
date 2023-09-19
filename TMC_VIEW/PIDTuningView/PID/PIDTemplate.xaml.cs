using System;
using System.Collections.Generic;
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

namespace TMC_VIEW.PIDTuningView.PID
{
    /// <summary>
    /// Interaction logic for PIDTemplate.xaml
    /// </summary>
    public partial class PIDTemplate : UserControl
    {
        public PIDTemplate()
        {
            InitializeComponent();
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header",
                typeof(string), typeof(PIDTemplate));


        public double kP
        {
            get { return (double)GetValue(kPProperty); }
            set { SetValue(kPProperty, value); }
        }


        public static readonly DependencyProperty kPProperty =
       DependencyProperty.Register("kP",
           typeof(double), typeof(PIDTemplate));



        public double kI
        {
            get { return (double)GetValue(kIProperty); }
            set { SetValue(kIProperty, value); }
        }


        public static readonly DependencyProperty kIProperty =
       DependencyProperty.Register("kI",
           typeof(double), typeof(PIDTemplate));


        public double kD
        {
            get { return (double)GetValue(kDProperty); }
            set { SetValue(kDProperty, value); }
        }


        public static readonly DependencyProperty kDProperty =
       DependencyProperty.Register("kD",
           typeof(double), typeof(PIDTemplate));

        public double Imax
        {
            get { return (double)GetValue(ImaxProperty); }
            set { SetValue(ImaxProperty, value); }
        }


        public static readonly DependencyProperty ImaxProperty =
       DependencyProperty.Register("Imax",
           typeof(double), typeof(PIDTemplate));


        public double L
        {
            get { return (double)GetValue(LProperty); }
            set { SetValue(LProperty, value); }
        }


        public static readonly DependencyProperty LProperty =
       DependencyProperty.Register("L",
           typeof(double), typeof(PIDTemplate));

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^-?[0-9]*(?:\.[0-9]*)?$");
            e.Handled = !regex.IsMatch(e.Text);

        }
    }
}

