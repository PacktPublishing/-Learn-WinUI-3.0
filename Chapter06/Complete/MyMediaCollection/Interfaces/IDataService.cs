using MyMediaCollection.Enums;
using MyMediaCollection.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMediaCollection.Interfaces
{
    public interface IDataService
    {
        Task InitializeDataAsync();
        IList<MediaItem> GetItems();
        MediaItem GetItem(int id);
        int AddItem(MediaItem item);
        void UpdateItem(MediaItem item);
        void DeleteItem(MediaItem item);
        IList<ItemType> GetItemTypes();
        Medium GetMedium(string name);
        IList<Medium> GetMediums();
        IList<Medium> GetMediums(ItemType itemType);
        IList<LocationType> GetLocationTypes();
        int SelectedItemId { get; set; }
    }
}