using Microsoft.UI.Xaml.Input;
using MyMediaCollection.Interfaces;
using MyMediaCollection.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMediaCollection.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private string selectedMedium;
        private ObservableCollection<MediaItem> items = new ObservableCollection<MediaItem>();
        private ObservableCollection<MediaItem> allItems;
        private ObservableCollection<string> mediums;
        private MediaItem selectedMediaItem;
        private const string AllMediums = "All";

        public MainViewModel(INavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;

            DeleteCommand = new RelayCommand(async () => await DeleteItemAsync(), CanDeleteItem);
            AddEditCommand = new RelayCommand(AddOrEditItem);

            PopulateDataAsync();
        }

        public async Task PopulateDataAsync()
        {
            items.Clear();
            
            foreach(var item in await _dataService.GetItemsAsync())
            {
                items.Add(item);
            }

            allItems = new ObservableCollection<MediaItem>(Items);

            mediums = new ObservableCollection<string>
            {
                AllMediums
            };

            foreach(var itemType in _dataService.GetItemTypes())
            {
                mediums.Add(itemType.ToString());
            }

            selectedMedium = Mediums[0];
        }

        public ObservableCollection<MediaItem> Items
        {
            get
            {
                return items;
            }
            set
            {
                SetProperty(ref items, value);
            }
        }

        public ObservableCollection<string> Mediums
        {
            get
            {
                return mediums;
            }
            set
            {
                SetProperty(ref mediums, value);
            }
        }

        public string SelectedMedium
        {
            get 
            {
                return selectedMedium;
            }
            set
            {
                SetProperty(ref selectedMedium, value);

                Items.Clear();
                allItems.Clear();
                IList<MediaItem> items = _dataService.GetItemsAsync().Result;

                foreach (var item in items)
                {
                    allItems.Add(item);
                }

                foreach (var item in from item in allItems
                                     where string.IsNullOrWhiteSpace(selectedMedium) ||
                                           selectedMedium == "All" ||
                                           selectedMedium == item.MediaType.ToString()
                                     select item)
                {
                    Items.Add(item);
                }
            }
        }

        public MediaItem SelectedMediaItem
        {
            get => selectedMediaItem;
            set
            {
                SetProperty(ref selectedMediaItem, value);
                ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand AddEditCommand { get; set; }

        private void AddOrEditItem()
        {
            var selectedItemId = -1;

            if (SelectedMediaItem != null)
            {
                selectedItemId = SelectedMediaItem.Id;
            }

            _navigationService.NavigateTo("ItemDetailsPage", selectedItemId);
        }

        public void ListViewDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            AddOrEditItem();
        }

        public ICommand DeleteCommand { get; set; }

        private async Task DeleteItemAsync()
        {
            await _dataService.DeleteItemAsync(SelectedMediaItem);
            Items.Remove(SelectedMediaItem);
            allItems.Remove(SelectedMediaItem);
        }

        private bool CanDeleteItem() => SelectedMediaItem != null;
    }
}