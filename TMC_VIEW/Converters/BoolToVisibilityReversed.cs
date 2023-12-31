﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace TMC_VIEW.Converters
{
    public class BoolToVisibilityReversed : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.GetType() == typeof(bool)) { 
            return ((bool)value == true) ? Visibility.Collapsed : Visibility.Visible;
            }
            if (value.GetType() == typeof(int))
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
