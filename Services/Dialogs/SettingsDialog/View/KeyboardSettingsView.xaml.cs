using Services.Dialogs.SettingsDialog.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Services.Dialogs.SettingsDialog.View
{
    /// <summary>
    /// Interaction logic for KeyboardSettingsView.xaml
    /// </summary>
    public partial class KeyboardSettingsView : UserControl
    {
        public KeyboardSettingsView()
        {
            InitializeComponent();
        }

        private Label? selectedLabel;
        private ListViewItem? selectedItem;
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lv = (ListView)sender;
            if (lv.SelectedItem != null)
            {
                object o = lv.SelectedItem;
                selectedItem = (ListViewItem)lv.ItemContainerGenerator.ContainerFromItem(o);

                selectedLabel = FindElementName("ValueLabel", selectedItem) as Label;
                if (selectedLabel == null)
                    return;
                selectedLabel.Content = "...";
                PreviewKeyDown += CaptureKey_PreviewKeyDown;
                // Label currentLabel = FindByName("ValueLabel", lvi) as Label;
            }
        }
        private FrameworkElement? FindElementName(string name, FrameworkElement root)
        {
            Stack<FrameworkElement> tree = new Stack<FrameworkElement>();
            tree.Push(root);

            while (tree.Count > 0)
            {
                FrameworkElement current = tree.Pop();
                if (current.Name == name)
                    return current;

                int count = VisualTreeHelper.GetChildrenCount(current);
                for (int i = 0; i < count; ++i)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(current, i);
                    if (child is FrameworkElement)
                        tree.Push((FrameworkElement)child);
                }
            }

            return null;
        }
        private void CaptureKey_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            SettingsDialogViewModel vm = (SettingsDialogViewModel)this.DataContext;
            if ((selectedItem == null) || selectedLabel == null)
                return;
            selectedLabel.Content = e.Key;
            vm.AppSettings.ControllersSettings.KeyboardAssignment[lv.SelectedIndex] = e.Key;
            PreviewKeyDown -= CaptureKey_PreviewKeyDown;
            lv.UnselectAll();
            //lv.Items.Refresh();

        }





        private void ListView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
