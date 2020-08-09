using Microsoft.Extensions.DependencyInjection;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WinUI2MediaCollection.ViewModels;

namespace WinUI2MediaCollection.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ItemDetailsPage : Page
    {
        public ItemDetailsPage()
        {
            this.InitializeComponent();

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string haveExplainedSaveSetting = localSettings.Values[nameof(SavingTip)] as string;

            if (!bool.TryParse(haveExplainedSaveSetting, out bool result) || !result)
            {
                SavingTip.IsOpen = true;
                localSettings.Values[nameof(SavingTip)] = "true";
            }

            Loaded += ItemDetailsPage_Loaded;
        }

        private void ItemDetailsPage_Loaded(object sender, RoutedEventArgs e)
        {
            var titleBar = ApplicationView.GetForCurrentView();
            titleBar.Title = "Item Details";
        }

        public ItemDetailsViewModel ViewModel { get; } = (Application.Current as App).Container.GetService<ItemDetailsViewModel>();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var selectedItemId = (int)e.Parameter;

            await ViewModel.InitializeItemDetailDataAsync(selectedItemId);
        }
    }
}