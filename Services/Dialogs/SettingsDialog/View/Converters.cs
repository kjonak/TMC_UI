using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using static Services.AppSettings.ControllersSettings;
using static Services.AppSettings.ControllersSettings.KeyboardControls;

/*
 * 
 *          ROLL_LEFT,
         ROLE_RIGHT,
         PITCH_UP,
         PITCH_DOWN,
         YAW_LEFT,
         YAW_RIGHT,
         FORWAD,
         BACKWARD,
         UP,
         DOWN,
         LEFT,
         RIGHT
 */
namespace Services.Dialogs.SettingsDialog.View
{
    public class IndexConverter : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {
            ListViewItem item = (ListViewItem)value;
            ListView? listView = ItemsControl.ItemsControlFromItemContainer(item) as ListView;
            int index = listView.ItemContainerGenerator.IndexFromContainer(item);
            switch ((KeyboardControls)index)
            {
                case ROLL_LEFT:
                    return "Roll left";
                    break;
                case ROLL_RIGHT:
                    return "Roll right";
                case PITCH_UP:
                    return "Pitch up";
                case PITCH_DOWN:
                    return "Pitch down";
                case YAW_LEFT:
                    return "Yaw left";
                case YAW_RIGHT:
                    return "Yaw right";
                case FORWAD:
                    return "Forward";
                case BACKWARD:
                    return "Backward";
                case UP:
                    return "Up";
                case DOWN:
                    return "Down";
                case LEFT:
                    return "Left";
                case RIGHT:
                    return "Right";

                default:
                    return "error";
            }
            return index.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
