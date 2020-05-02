using Microsoft.UI.Xaml.Input;
using MyMediaCollection.Enums;
using MyMediaCollection.Interfaces;
using MyMediaCollection.Model;

namespace MyMediaCollection.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private string selectedMedium;
        private TestObservableCollection<MediaItem> items;
        private TestObservableCollection<MediaItem> allItems;
        private TestObservableCollection<string> mediums;
        private MediaItem selectedMediaItem;
        private int additionalItemCount = 1;
        private INavigationService _navigationService;
        private IDataService _dataService;

        public MainViewModel(INavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;

            PopulateData();

            DeleteCommand = new RelayCommand(DeleteItem, CanDeleteItem);
            AddEditCommand = new RelayCommand(AddOrEditItem);
        }

        public void PopulateData()
        {
            items = new TestObservableCollection<MediaItem>();

            foreach(var item in _dataService.GetItems())
            {
                items.Add(item);
            }

            allItems = new TestObservableCollection<MediaItem>(Items);

            mediums = new TestObservableCollection<string>
            {
                "All"
            };

            foreach(var itemType in _dataService.GetItemTypes())
            {
                mediums.Add(itemType.ToString());
            }

            selectedMedium = Mediums[0];
        }

        public TestObservableCollection<MediaItem> Items
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

        public TestObservableCollection<string> Mediums
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

                foreach (var item in allItems)
                {
                    if (string.IsNullOrWhiteSpace(selectedMedium) ||
                        selectedMedium == "All" ||
                        selectedMedium == item.MediaType.ToString())
                    {
                        Items.Add(item);
                    }
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
            //var newItem = new MediaItem
            //{
            //    Id = 3 + additionalItemCount,
            //    Location = LocationType.InCollection,
            //    MediaType = (ItemType)(additionalItemCount % 3),
            //    MediumInfo = new Medium { Id = 1, MediaType = ItemType.Music, Name = "CD" },
            //    Name = $"CD {additionalItemCount}"
            //};

            //Items.Add(newItem);
            //additionalItemCount++;

            var selectedItemId = -1;

            if (SelectedMediaItem != null)
                selectedItemId = SelectedMediaItem.Id;

            _dataService.SelectedItemId = selectedItemId;
            _navigationService.NavigateTo("ItemDetailsPage");
        }

        public void ListViewDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            AddOrEditItem();
        }

        public ICommand DeleteCommand { get; set; }

        private void DeleteItem()
        {
            Items.Remove(SelectedMediaItem);
        }

        private bool CanDeleteItem() => SelectedMediaItem != null;
    }
}