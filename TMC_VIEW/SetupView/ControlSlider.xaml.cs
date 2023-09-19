using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace TMC_VIEW.SetupView
{
    /// <summary>
    /// Interaction logic for ControlSlider.xaml
    /// </summary>
    public partial class ControlSlider : UserControl
    {
        public ControlSlider()
        {
            InitializeComponent();
            MinValue = -1;
            MaxValue = 1;
            TicksCnt = 40;
        }
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
           DependencyProperty.Register("Value",
               typeof(double),
               typeof(ControlSlider));


        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        public static readonly DependencyProperty MinValueProperty =
           DependencyProperty.Register("MinValue",
               typeof(double),
               typeof(ControlSlider));

        public double TicksResolution
        {
            get { return (MaxValue - MinValue) / (double)TicksCnt; ; }
            set { }
        }

        public double TicksCnt
        {
            get { return (double)GetValue(TicksCntProperty); }
            set { SetValue(TicksCntProperty, value); }
        }
        public static readonly DependencyProperty TicksCntProperty =
           DependencyProperty.Register("TicksCnt",
               typeof(double),
               typeof(ControlSlider));

        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
        public static readonly DependencyProperty MaxValueProperty =
           DependencyProperty.Register("MaxValue",
               typeof(double),
               typeof(ControlSlider));
    }
}
