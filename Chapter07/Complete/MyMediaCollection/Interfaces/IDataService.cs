using MyMediaCollection.Enums;
using MyMediaCollection.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMediaCollection.Interfaces
{
    public interface IDataService
    {
        Task InitializeDataAsync();
        Task<IList<MediaItem>> GetItemsAsync();
        Task<MediaItem> GetItemAsync(int id);
        Task<int> AddItemAsync(MediaItem item);
        Task UpdateItemAsync(MediaItem item);
        Task DeleteItemAsync(MediaItem item);
        IList<ItemType> GetItemTypes();
        Medium GetMedium(string name);
        Medium GetMedium(int id);
        IList<Medium> GetMediums();
        IList<Medium> GetMediums(ItemType itemType);
        IList<LocationType> GetLocationTypes();
    }
}