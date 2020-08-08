namespace WebViewBrowser.Bus.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _urlSource = "https://www.packtpub.com/";

        public string UrlSource
        {
            get
            {
                return _urlSource;
            }
            set
            {
                SetProperty(ref _urlSource, value, nameof(UrlSource));
            }
        }
    }
}