using MyMediaCollection.Enums;
using MyMediaCollection.Interfaces;
using MyMediaCollection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMediaCollection.Services
{
    public class DataService : IDataService
    {
        private IList<MediaItem> _items;
        private IList<ItemType> _itemTypes;
        private IList<Medium> _mediums;
        private IList<LocationType> _locationTypes;

        public async Task InitializeDataAsync()
        {
            PopulateItemTypes();
            PopulateMediums();
            PopulateLocationTypes();
            PopulateItems();
        }

        private void PopulateItems()
        {
            var cd = new MediaItem
            {
                Id = 1,
                Name = "Classical Favorites",
                MediaType = ItemType.Music,
                MediumInfo = _mediums.FirstOrDefault(m => m.Name == "CD"),
                Location = LocationType.InCollection
            };

            var book = new MediaItem
            {
                Id = 2,
                Name = "Classic Fairy Tales",
                MediaType = ItemType.Book,
                MediumInfo = _mediums.FirstOrDefault(m => m.Name == "Hardcover"),
                Location = LocationType.InCollection
            };

            var bluRay = new MediaItem
            {
                Id = 3,
                Name = "The Mummy",
                MediaType = ItemType.Video,
                MediumInfo = _mediums.FirstOrDefault(m => m.Name == "Blu Ray"),
                Location = LocationType.InCollection
            };

            _items = new List<MediaItem>
            {
                cd,
                book,
                bluRay
            };
        }

        private void PopulateMediums()
        {
            var cd = new Medium { Id = 1, MediaType = ItemType.Music, Name = "CD" };
            var vinyl = new Medium { Id = 2, MediaType = ItemType.Music, Name = "Vinyl" };
            var hardcover = new Medium { Id = 3, MediaType = ItemType.Book, Name = "Hardcover" };
            var paperback = new Medium { Id = 4, MediaType = ItemType.Book, Name = "Paperback" };
            var dvd = new Medium { Id = 5, MediaType = ItemType.Video, Name = "DVD" };
            var bluRay = new Medium { Id = 6, MediaType = ItemType.Video, Name = "Blu Ray" };

            _mediums = new List<Medium>
            {
                cd,
                vinyl,
                hardcover,
                paperback,
                dvd,
                bluRay
            };
        }

        private void PopulateItemTypes()
        {
            _itemTypes = new List<ItemType>
            {
                ItemType.Book,
                ItemType.Music,
                ItemType.Video
            };
        }

        private void PopulateLocationTypes()
        {
            _locationTypes = new List<LocationType>
            {
                LocationType.InCollection,
                LocationType.Loaned
            };
        }

        public async Task<int> AddItemAsync(MediaItem item)
        {
            item.Id = _items.Max(i => i.Id) + 1;
            _items.Add(item);

            return item.Id;
        }

        public async Task<MediaItem> GetItemAsync(int id)
        {
            return _items.FirstOrDefault(i => i.Id == id);
        }

        public async Task<IList<MediaItem>> GetItemsAsync()
        {
            return _items;
        }

        public IList<ItemType> GetItemTypes()
        {
            return _itemTypes;
        }

        public IList<Medium> GetMediums()
        {
            return _mediums;
        }

        public IList<Medium> GetMediums(ItemType itemType)
        {
            return _mediums
                .Where(m => m.MediaType == itemType)
                .ToList();
        }

        public IList<LocationType> GetLocationTypes()
        {
            return _locationTypes;
        }

        public async Task UpdateItemAsync(MediaItem item)
        {
            var idx = -1;
            var matchedItem =
                (from x in _items
                 let ind = idx++
                 where x.Id == item.Id
                 select ind).FirstOrDefault();

            if (idx == -1)
            {
                throw new Exception("Unable to update item. Item not found in collection.");
            }

            _items[idx] = item;
        }

        public Medium GetMedium(string name)
        {
            return _mediums.FirstOrDefault(m => m.Name == name);
        }

        public Medium GetMedium(int id)
        {
            return _mediums.FirstOrDefault(m => m.Id == id);
        }

        public async Task DeleteItemAsync(MediaItem item)
        {
            _items.Remove(item);
        }
    }
}