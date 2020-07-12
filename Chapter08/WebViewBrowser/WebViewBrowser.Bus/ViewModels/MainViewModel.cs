using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebViewBrowser.Bus.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _urlSource;

        public MainViewModel()
        {
            UrlSource = "https://www.packtpub.com/";
        }

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
