using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;
using WinUI2MediaCollection.Enums;
using WinUI2MediaCollection.Interfaces;
using WinUI2MediaCollection.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace WinUI2MediaCollection.Services
{
    public class SqliteDataService : IDataService
    {
        #region Private Data Members

        private IList<ItemType> _itemTypes;
        private IList<Medium> _mediums;
        private IList<LocationType> _locationTypes;
        private const string DbName = "mediaCollectionData.db";

        #endregion

        #region Public Methods

        public async Task<int> AddItemAsync(MediaItem item)
        {
            using (var db = await GetOpenConnectionAsync())
            {
                return await InsertMediaItemAsync(db, item);
            }
        }

        public async Task UpdateItemAsync(MediaItem item)
        {
            using (var db = await GetOpenConnectionAsync())
            {
                await UpdateMediaItemAsync(db, item);
            }
        }

        public async Task DeleteItemAsync(MediaItem item)
        {
            using (var db = await GetOpenConnectionAsync())
            {
                await DeleteMediaItemAsync(db, item.Id);
            }
        }

        public async Task<MediaItem> GetItemAsync(int id)
        {
            IList<MediaItem> mediaItems;

            using (var db = await GetOpenConnectionAsync())
            {
                mediaItems = await GetAllMediaItemsAsync(db);
            }

            // Filter the list to get the item for our Id.
            return mediaItems.FirstOrDefault(i => i.Id == id);
        }

        public async Task<IList<MediaItem>> GetItemsAsync()
        {
            using (var db = await GetOpenConnectionAsync())
            {
                return await GetAllMediaItemsAsync(db);
            }
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

        public Medium GetMedium(string name)
        {
            return _mediums.FirstOrDefault(m => m.Name == name);
        }

        public Medium GetMedium(int id)
        {
            return _mediums.FirstOrDefault(m => m.Id == id);
        }

        public async Task InitializeDataAsync()
        {
            using (var db = await GetOpenConnectionAsync())
            {
                await CreateMediumTableAsync(db);
                await CreateMediaItemTableAsync(db);

                PopulateItemTypes();
                await PopulateMediumsAsync(db);
                PopulateLocationTypes();
            }
        }

        #endregion

        #region Private Methods

        private async Task PopulateMediumsAsync(SqliteConnection db)
        {
            _mediums = await GetAllMediumsAsync(db);

            if (_mediums.Count == 0)
            {
                var cd = new Medium { Id = 1, MediaType = ItemType.Music, Name = "CD" };
                var vinyl = new Medium { Id = 2, MediaType = ItemType.Music, Name = "Vinyl" };
                var hardcover = new Medium { Id = 3, MediaType = ItemType.Book, Name = "Hardcover" };
                var paperback = new Medium { Id = 4, MediaType = ItemType.Book, Name = "Paperback" };
                var dvd = new Medium { Id = 5, MediaType = ItemType.Video, Name = "DVD" };
                var bluRay = new Medium { Id = 6, MediaType = ItemType.Video, Name = "Blu Ray" };

                var mediums = new List<Medium>
                {
                    cd,
                    vinyl,
                    hardcover,
                    paperback,
                    dvd,
                    bluRay
                };

                foreach (var medium in mediums)
                {
                    await InsertMediumAsync(db, medium);
                }

                _mediums = await GetAllMediumsAsync(db);
            }
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

        private async Task<SqliteConnection> GetOpenConnectionAsync()
        {
            await ApplicationData.Current.RoamingFolder.CreateFileAsync(DbName, CreationCollisionOption.OpenIfExists).AsTask().ConfigureAwait(false);
            string dbPath = Path.Combine(ApplicationData.Current.RoamingFolder.Path, DbName);
            var cn = new SqliteConnection($"Filename={dbPath}");
            cn.Open();

            return cn;
        }

        private async Task CreateMediumTableAsync(SqliteConnection db)
        {
            string tableCommand = @"CREATE TABLE IF NOT 
                EXISTS Mediums (Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                Name NVARCHAR(30) NOT NULL, 
                MediumType INTEGER NOT NULL)";

            var createTable = new SqliteCommand(tableCommand, db);

            await createTable.ExecuteNonQueryAsync();
        }

        private async Task CreateMediaItemTableAsync(SqliteConnection db)
        {
            string tableCommand = @"CREATE TABLE IF NOT 
                EXISTS MediaItems (Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                Name NVARCHAR(1000) NOT NULL, 
                ItemType INTEGER NOT NULL, 
                MediumId INTEGER NOT NULL, 
                LocationType INTEGER, 
                CONSTRAINT fk_mediums 
                FOREIGN KEY(MediumId) 
                REFERENCES Mediums(Id))";

            var createTable = new SqliteCommand(tableCommand, db);

            await createTable.ExecuteNonQueryAsync();
        }

        private async Task InsertMediumAsync(SqliteConnection db, Medium medium)
        {
            var newIds = await db.QueryAsync<long>(
                $@"INSERT INTO Mediums
                    ({nameof(medium.Name)}, MediumType)
                    VALUES
                    (@{nameof(medium.Name)}, @{nameof(medium.MediaType)});
                SELECT last_insert_rowid()", medium);

            medium.Id = (int)newIds.First();
        }

        private async Task<IList<Medium>> GetAllMediumsAsync(SqliteConnection db)
        {
            var mediums = 
                await db.QueryAsync<Medium>(@"SELECT Id, 
                                                     Name, 
                                                     MediumType AS MediaType
                                              FROM Mediums");

            return mediums.ToList();
        }

        private async Task<List<MediaItem>> GetAllMediaItemsAsync(SqliteConnection db)
        {
            var itemsResult = await db.QueryAsync<MediaItem, Medium, MediaItem>
                            (
                                @"SELECT
                                    [MediaItems].[Id],
                                    [MediaItems].[Name],
                                    [MediaItems].[ItemType] AS MediaType,
                                    [MediaItems].[LocationType] AS Location,
                                    [Mediums].[Id],
                                    [Mediums].[Name],
                                    [Mediums].[MediumType] AS MediaType
                                FROM
                                    [MediaItems]
                                JOIN
                                    [Mediums]
                                ON
                                    [Mediums].[Id] = [MediaItems].[MediumId]",
                                (item, medium) =>
                                {
                                    item.MediumInfo = medium;

                                    return item;
                                }
                            );

            return itemsResult.ToList();
        }

        private async Task<int> InsertMediaItemAsync(SqliteConnection db, MediaItem item)
        {
            var newIds = await db.QueryAsync<long>(
                @"INSERT INTO MediaItems
                    (Name, ItemType, MediumId, LocationType)
                    VALUES
                    (@Name, @MediaType, @MediumId, @Location);
                SELECT last_insert_rowid()", item);

            return (int)newIds.First();
        }

        private async Task UpdateMediaItemAsync(SqliteConnection db, MediaItem item)
        {
            await db.QueryAsync(
                @"UPDATE MediaItems
                  SET Name = @Name,
                      ItemType = @MediaType,
                      MediumId = @MediumId,
                      LocationType = @Location
                  WHERE Id = @Id;", item);
        }

        private async Task DeleteMediaItemAsync(SqliteConnection db, int id)
        {
            await db.DeleteAsync<MediaItem>(new MediaItem { Id = id });
        }

        #endregion
    }
}
