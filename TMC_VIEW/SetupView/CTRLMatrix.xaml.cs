using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
using TMC_VIEW_MODEL;

namespace TMC_VIEW.SetupView
{
    /// <summary>
    /// Interaction logic for CTRLMatrix.xaml
    /// </summary>
    /// 
    public class FloatBox :TextBox
    {
        public int Index;
        public FloatBox(int index) 
        {
            Index = index;
        }
    }

    public partial class CTRLMatrix : UserControl
    {
        private static readonly string[] _dof_names = { "Roll", "Pitch", "Yaw", "Vertical", "Longitudinal", "Lateral" };
        public CTRLMatrix()
        {
            InitializeComponent();
            ValueGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            ValueGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            for (int i = 0; i < 6; i++)
            {
                ValueGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                Label l = new Label();
                l.Content = _dof_names[i];
                ValueGrid.Children.Add(l);
                Grid.SetRow(l, i + 1);
            }

            for (int i = 0; i < 8; i++)
            {
                ValueGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                Label l = new Label();
                l.Content = "M" + (i + 1).ToString();
                l.HorizontalContentAlignment = HorizontalAlignment.Center;
                ValueGrid.Children.Add(l);
                Grid.SetColumn(l, i + 1);
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    FloatBox tb = new FloatBox(i + j*8);
                    ValueGrid.Children.Add(tb);
                    tb.PreviewTextInput += Tb_PreviewTextInput;
                    tb.TextChanged += Tb_TextChanged;
                    tb.Text = "0";
                    tb.Margin = new Thickness(5);
                    tb.HorizontalAlignment = HorizontalAlignment.Stretch;
                    tb.HorizontalContentAlignment = HorizontalAlignment.Center;
                    tb.VerticalAlignment = VerticalAlignment.Center;
                    tb.VerticalContentAlignment = VerticalAlignment.Center;

                    Grid.SetColumn(tb, i + 1);
                    Grid.SetRow(tb, j+1);

                }

                ValueGrid.ColumnDefinitions[i + 1].Width = new GridLength(50);
            }

         

        }

        private void Tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetupViewModel viewModel = (SetupViewModel)this.DataContext;
            if(viewModel == null ) { return; }
            var selectedBox = sender as FloatBox;
            if(selectedBox == null)
                return;

            float value;
            string text = selectedBox.Text.Replace(".", ",");
            if (!float.TryParse(text, out value))
                value = 0;

            viewModel.CTRLMatrix[selectedBox.Index] = value;
        }

        private void Tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^-?[0-9]*(?:\.[0-9]*)?$");
            e.Handled = !regex.IsMatch(e.Text);
        }

 
    }
}
