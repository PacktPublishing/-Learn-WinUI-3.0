using ABI.Windows.UI.Popups;
using Microsoft.UI.Xaml;
using WebViewBrowser.Bus.ViewModels;

namespace WebViewBrowser
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            mainWebView.Source = browserToolbar.UrlSource;
        }

        public MainViewModel ViewModel { get; } = new MainViewModel();

        private void browserToolbar_ReloadClicked(object sender, RoutedEventArgs e)
        {
            mainWebView.Reload();
        }

        private void browserToolbar_UrlEntered(object sender, RoutedEventArgs e)
        {
            mainWebView.Source = browserToolbar.UrlSource;
        }
    }
}