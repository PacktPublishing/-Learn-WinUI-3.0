using Microsoft.UI.Xaml.Input;
using MyMediaCollection.Enums;
using MyMediaCollection.Interfaces;
using MyMediaCollection.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMediaCollection.ViewModels
{
    public class ItemDetailsViewModel : BindableBase
    {
        private TestObservableCollection<string> _locationTypes = new TestObservableCollection<string>();
        private TestObservableCollection<string> _mediums = new TestObservableCollection<string>();
        private TestObservableCollection<string> _itemTypes = new TestObservableCollection<string>();
        private int _itemId;
        private string _itemName;
        private string _selectedMedium;
        private string _selectedItemType;
        private string _selectedLocation;
        private bool _isDirty;
        private int _selectedItemId = -1;

        public ItemDetailsViewModel(INavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;

            CancelCommand = new RelayCommand(Cancel);
        }

        public async Task InitializeItemDetailDataAsync(int selectedItemId)
        {
            _selectedItemId = selectedItemId;
            PopulateLists();

            if (_selectedItemId > 0)
            {
                await PopulateExistingItemAsync(_dataService);
            }

            IsDirty = false;
        }

        private async Task PopulateExistingItemAsync(IDataService dataService)
        {
            if (_selectedItemId > 0)
            {
                var item = await _dataService.GetItemAsync(_selectedItemId);
                Mediums.Clear();

                foreach (string medium in dataService.GetMediums(item.MediaType).Select(m => m.Name))
                    Mediums.Add(medium);

                _itemId = item.Id;
                ItemName = item.Name;
                SelectedMedium = item.MediumInfo.Name;
                SelectedLocation = item.Location.ToString();
                SelectedItemType = item.MediaType.ToString();
            }
        }

        private void PopulateLists()
        {
            ItemTypes.Clear();
            foreach (string iType in Enum.GetNames(typeof(ItemType)))
                ItemTypes.Add(iType);

            LocationTypes.Clear();
            foreach (string lType in Enum.GetNames(typeof(LocationType)))
                LocationTypes.Add(lType);

            Mediums = new TestObservableCollection<string>();
        }

        public async Task SaveItemAndReturnAsync()
        {
            await SaveItemAsync();

            _navigationService.GoBack();
        }

        private async Task SaveItemAsync()
        {
            MediaItem item;

            if (_itemId > 0)
            {
                item = await _dataService.GetItemAsync(_itemId);

                item.Name = ItemName;
                item.Location = (LocationType)Enum.Parse(typeof(LocationType), SelectedLocation);
                item.MediaType = (ItemType)Enum.Parse(typeof(ItemType), SelectedItemType);
                item.MediumInfo = _dataService.GetMedium(SelectedMedium);

                await _dataService.UpdateItemAsync(item);
            }
            else
            {
                item = new MediaItem
                {
                    Name = ItemName,
                    Location = (LocationType)Enum.Parse(typeof(LocationType), SelectedLocation),
                    MediaType = (ItemType)Enum.Parse(typeof(ItemType), SelectedItemType),
                    MediumInfo = _dataService.GetMedium(SelectedMedium)
                };

                await _dataService.AddItemAsync(item);
            }
        }

        public async Task SaveItemAndContinueAsync()
        {
            await SaveItemAsync();
            _itemId = 0;
            ItemName = "";
            SelectedMedium = null;
            SelectedLocation = null;
            SelectedItemType = null;
            IsDirty = false;
        }

        public ICommand CancelCommand { get; set; }

        [MinLength(2, ErrorMessage ="Item name must be at least 2 characters.")]
        [MaxLength(100, ErrorMessage ="Item name must be 100 characters or less.")]
        public string ItemName
        {
            get => _itemName;
            set
            {
                if (!SetProperty(ref _itemName, value, nameof(ItemName)))
                    return;

                IsDirty = true;
            }
        }

        public string SelectedMedium 
        {
            get => _selectedMedium; 
            set
            {
                if (!SetProperty(ref _selectedMedium, value, nameof(SelectedMedium)))
                    return;

                IsDirty = true;
            }
        }

        public string SelectedItemType 
        {
            get => _selectedItemType; 
            set
            {
                if (!SetProperty(ref _selectedItemType, value, nameof(SelectedItemType)))
                    return;

                IsDirty = true;

                Mediums.Clear();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    foreach (string med in _dataService.GetMediums((ItemType)Enum.Parse(typeof(ItemType), SelectedItemType)).Select(m => m.Name))
                        Mediums.Add(med);
                }
            }
        }

        public string SelectedLocation
        {
            get => _selectedLocation; 
            set
            {
                if (!SetProperty(ref _selectedLocation, value, nameof(SelectedLocation)))
                    return;

                IsDirty = true;
            }
        }

        public TestObservableCollection<string> LocationTypes { get => _locationTypes; set => SetProperty(ref _locationTypes, value, nameof(LocationTypes)); }
        public TestObservableCollection<string> Mediums { get => _mediums; set => SetProperty(ref _mediums, value, nameof(Mediums)); }
        public TestObservableCollection<string> ItemTypes { get => _itemTypes; set => SetProperty(ref _itemTypes, value, nameof(ItemTypes)); }

        public bool IsDirty
        {
            get => _isDirty; 
            set
            {
                SetProperty(ref _isDirty, value, nameof(IsDirty));
            }
        }

        private void Cancel()
        {
            _navigationService.GoBack();
        }
    }
}