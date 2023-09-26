using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
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
        DependencyProperty.Register(
            name: "kP",
            propertyType: typeof(double),
            ownerType: typeof(PIDTemplate),
            typeMetadata: new FrameworkPropertyMetadata((double)0,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public double kI
        {
            get { return (double)GetValue(kIProperty); }
            set { SetValue(kIProperty, value); }
        }


        public static readonly DependencyProperty kIProperty =
        DependencyProperty.Register(
            name:"kI",
            propertyType: typeof(double),
            ownerType: typeof(PIDTemplate),
            typeMetadata: new FrameworkPropertyMetadata((double)0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double kD
        {
            get { return (double)GetValue(kDProperty); }
            set { 

                SetValue(kDProperty, value); 
            }
        }


        public static readonly DependencyProperty kDProperty =
        DependencyProperty.Register(
            name: "kD",
            propertyType: typeof(double),
            ownerType: typeof(PIDTemplate),
            typeMetadata: new FrameworkPropertyMetadata((double)0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double Imax
        {
            get { return (double)GetValue(ImaxProperty); }
            set { SetValue(ImaxProperty, value); }
        }


        public static readonly DependencyProperty ImaxProperty =
        DependencyProperty.Register(
            name: "Imax",
            propertyType: typeof(double),
            ownerType: typeof(PIDTemplate),
            typeMetadata: new FrameworkPropertyMetadata((double)0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double L
        {
            get { return (double)GetValue(LProperty); }
            set {  SetValue(LProperty, value);}
        }


        public static readonly DependencyProperty LProperty =
        DependencyProperty.Register(
            name: "L",
            propertyType: typeof(double),
            ownerType: typeof(PIDTemplate),
            typeMetadata: new FrameworkPropertyMetadata((double)0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^-?[0-9]*(?:\.[0-9]*)?$");
            e.Handled = !regex.IsMatch(e.Text);

        }

    }
}

