using Microsoft.UI.Xaml.Input;
using MyMediaCollection.Enums;
using MyMediaCollection.Interfaces;
using MyMediaCollection.Model;
using System;
using System.Linq;

namespace MyMediaCollection.ViewModels
{
    public class ItemDetailsViewModel : BindableBase
    {
        private INavigationService _navigationService;
        private IDataService _dataService;
        private TestObservableCollection<string> _locationTypes = new TestObservableCollection<string>();
        private TestObservableCollection<string> _mediums = new TestObservableCollection<string>();
        private TestObservableCollection<string> _itemTypes = new TestObservableCollection<string>();
        private int _itemId;
        private string _itemName;
        private string _selectedMedium;
        private string _selectedItemType;
        private string _selectedLocation;
        private bool _isDirty;

        public ItemDetailsViewModel(INavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;

            SaveCommand = new RelayCommand(SaveItemAndReturn, CanSaveItem);
            SaveAndContinueCommand = new RelayCommand(SaveItemAndContinue, CanSaveItem);
            CancelCommand = new RelayCommand(Cancel);

            PopulateLists();
            PopulateExistingItem(dataService);

            IsDirty = false;
        }

        private void PopulateExistingItem(IDataService dataService)
        {
            if (_dataService.SelectedItemId > 0)
            {
                var item = _dataService.GetItem(_dataService.SelectedItemId);
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

        public ICommand SaveCommand { get; set; }

        private void SaveItemAndReturn()
        {
            SaveItem();

            _navigationService.GoBack();
        }

        private void SaveItem()
        {
            MediaItem item;

            if (_itemId > 0)
            {
                item = _dataService.GetItem(_itemId);

                item.Name = ItemName;
                item.Location = (LocationType)Enum.Parse(typeof(LocationType), SelectedLocation);
                item.MediaType = (ItemType)Enum.Parse(typeof(ItemType), SelectedItemType);
                item.MediumInfo = _dataService.GetMedium(SelectedMedium);

                _dataService.UpdateItem(item);
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

                _dataService.AddItem(item);
            }
        }

        private bool CanSaveItem()
        {
            return IsDirty;
        }

        public ICommand SaveAndContinueCommand { get; set; }

        private void SaveItemAndContinue()
        {
            SaveItem();
            _dataService.SelectedItemId = 0;
            _itemId = 0;
            ItemName = "";
            SelectedMedium = null;
            SelectedLocation = null;
            SelectedItemType = null;
            IsDirty = false;
        }

        public ICommand CancelCommand { get; set; }

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
                if (_isDirty == value) return;

                _isDirty = value;
                ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
                ((RelayCommand)SaveAndContinueCommand).RaiseCanExecuteChanged();
            }
        }

        private void Cancel()
        {
            _navigationService.GoBack();
        }
    }
}