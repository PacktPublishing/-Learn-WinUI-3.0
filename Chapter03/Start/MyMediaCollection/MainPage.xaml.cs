using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using MyMediaCollection.Model;
using MyMediaCollection.Enums;
using Windows.UI.Popups;
using System.Linq;

namespace MyMediaCollection
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public IList<MediaItem> _items { get; set; }
        private IList<MediaItem> _allItems { get; set; }
        private IList<string> _mediums { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            ItemList.Loaded += ItemList_Loaded;
            ItemFilter.Loaded += ItemFilter_Loaded;
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ItemFilter.SelectionChanged += ItemFilter_SelectionChanged;
            PopulateData();
        }

        private async void AddButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Adding items to the collection is not yet available.", "My Media Collection");
            await dialog.ShowAsync();
        }

        public void PopulateData()
        {
            var cd = new MediaItem
            {
                Id = 1,
                Name = "Classical Favorites",
                MediaType = ItemType.Music,
                MediumInfo = new Medium { Id = 1, MediaType = ItemType.Music, Name = "CD" }
            };

            var book = new MediaItem
            {
                Id = 2,
                Name = "Classic Fairy Tales",
                MediaType = ItemType.Book,
                MediumInfo = new Medium { Id = 2, MediaType = ItemType.Book, Name = "Book" }
            };

            var bluRay = new MediaItem
            {
                Id = 3,
                Name = "The Mummy",
                MediaType = ItemType.Video,
                MediumInfo = new Medium { Id = 3, MediaType = ItemType.Video, Name = "Blu Ray" }
            };

            _items = new List<MediaItem>
            {
                cd,
                book,
                bluRay
            };

            _allItems = new List<MediaItem>
            {
                cd,
                book,
                bluRay
            };

            _mediums = new List<string>
            {
                "All",
                nameof(ItemType.Book),
                nameof(ItemType.Music),
                nameof(ItemType.Video)
            };
        }

        private void ItemList_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var listView = (ListView)sender;
            listView.ItemsSource = _items;
        }

        private void ItemFilter_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var filterCombo = (ComboBox)sender;
            filterCombo.ItemsSource = _mediums;
            filterCombo.SelectedIndex = 0;
        }

        private void ItemFilter_SelectionChanged(object sender, Microsoft.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            var updatedItems = (from item in _allItems
                                where string.IsNullOrWhiteSpace(ItemFilter.SelectedValue.ToString()) ||
                                      ItemFilter.SelectedValue.ToString() == "All" ||
                                      ItemFilter.SelectedValue.ToString() == item.MediaType.ToString()
                                select item).ToList();
            ItemList.ItemsSource = updatedItems;
        }
    }
}