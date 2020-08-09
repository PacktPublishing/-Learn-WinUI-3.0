using Microsoft.Extensions.DependencyInjection;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinUI2MediaCollection.ViewModels;

namespace WinUI2MediaCollection
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var titleBar = ApplicationView.GetForCurrentView();
            titleBar.Title = "Home";
        }

        public MainViewModel ViewModel { get; } = (Application.Current as App).Container.GetService<MainViewModel>();
    }
}