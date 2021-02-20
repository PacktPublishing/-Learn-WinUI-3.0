using System.ComponentModel;

namespace WinUI.SimpleSample
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _name;

        public MainViewModel()
        {
            Name = "Bob Jones";
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name == value) return;

                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }
    }
}
