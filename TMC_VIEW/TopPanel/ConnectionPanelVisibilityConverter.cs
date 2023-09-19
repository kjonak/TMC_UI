using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TMC_VIEW.TopPanel
{
    public class ConnectionPanelVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(parameter == null)
            {
                throw new NotImplementedException();
            }
            if((string)parameter == "UDP")
            {
                return ((int)value == 1) ? Visibility.Visible : Visibility.Collapsed;
            }
            else if((string)parameter =="Serial")
            {
                return ((int)value == 1) ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                throw new NotImplementedException();
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
