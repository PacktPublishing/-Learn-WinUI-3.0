using Microsoft.UI.Xaml.Controls;

namespace WinUI.SimpleSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.ViewModel = new MainViewModel();
        }

        public MainViewModel ViewModel { get; set; }
    }
}