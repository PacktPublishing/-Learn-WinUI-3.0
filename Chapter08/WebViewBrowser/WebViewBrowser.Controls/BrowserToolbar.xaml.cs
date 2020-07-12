using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace WebViewBrowser.Controls
{
    public sealed partial class BrowserToolbar : UserControl
    {
        private const string InitialUrl = "https://www.packtpub.com/";

        public event RoutedEventHandler ReloadClicked;
        public event RoutedEventHandler UrlEntered;
        
        public BrowserToolbar()
        {
            this.InitializeComponent();

            urlTextBox.Text = InitialUrl;
            UrlSource = new System.Uri(InitialUrl);
            UrlEntered?.Invoke(this, new RoutedEventArgs());
        }

        public void urlTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter &&
                !string.IsNullOrWhiteSpace(urlTextBox.Text))
            {
                UrlSource = new System.Uri(urlTextBox.Text);
                UrlEntered?.Invoke(this, new RoutedEventArgs());
            }
        }

        private void reloadButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadClicked?.Invoke(this, new RoutedEventArgs());
        }

        public static readonly DependencyProperty UrlSourceProperty = 
            DependencyProperty.Register(nameof(UrlSource),
                                        typeof(System.Uri),
                                        typeof(BrowserToolbar),
                                        new PropertyMetadata(null));

        public System.Uri UrlSource
        {
            get { return (System.Uri)GetValue(UrlSourceProperty); }
            set { SetValue(UrlSourceProperty, value); }
        }
    }
}