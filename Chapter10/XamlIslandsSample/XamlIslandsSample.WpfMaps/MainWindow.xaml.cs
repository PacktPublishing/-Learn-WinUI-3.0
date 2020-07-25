using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
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

namespace XamlIslandsSample.WpfMaps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void location_Click(object sender, RoutedEventArgs e)
        {
            var button = e.Source as Button;

            if (button == null) return;

            BasicGeoposition position = new BasicGeoposition() { Latitude = 0, Longitude = 0 };

            // Specify the location based on the button clicked
            switch (button.Name)
            {
                case nameof(parisButton):
                    position = new BasicGeoposition() { Latitude = 48.858242, Longitude = 2.2949378 };
                    break;
                case nameof(londonButton):
                    position = new BasicGeoposition() { Latitude = 51.502716, Longitude = -0.119304 };
                    break;
                case nameof(newYorkButton):
                    position = new BasicGeoposition() { Latitude = 40.748463, Longitude = -73.98567 };
                    break;
                case nameof(chinaButton):
                    position = new BasicGeoposition() { Latitude = 40.67693, Longitude = 117.23193 };
                    break;
            }

            var point = new Geopoint(position);

            // Set the map location
            await mapControl.TrySetViewAsync(point, 12);
        }

        private void wallInfoButton_Click(object sender, RoutedEventArgs e)
        {
            var browserWindow = new BrowserWindow();
            browserWindow.ShowDialog();
        }
    }
}