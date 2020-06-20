using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using MyMediaCollection.ViewModels;

namespace MyMediaCollection.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ItemDetailsPage : Page
    {
        public ItemDetailsPage()
        {
            this.InitializeComponent();
        }

        public ItemDetailsViewModel ViewModel { get; } = (Application.Current as App)?.Container.GetService<ItemDetailsViewModel>();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var selectedItemId = (int)e.Parameter;

            if (selectedItemId > 0)
            {
                ViewModel.InitializeItemDetailData(selectedItemId);
            }
        }
    }
}