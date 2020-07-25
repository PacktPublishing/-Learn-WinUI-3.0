using System.Windows;

namespace XamlIslandsSample.WpfCore
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

        private void quantityButton_Click(object sender, RoutedEventArgs e)
        {
            var entryControl = xamlHost.GetUwpInternalObject() as UwpApp.EntryForm;
            
            MessageBox.Show("Total entries: " + entryControl.QuantitySaved);
        }
    }
}