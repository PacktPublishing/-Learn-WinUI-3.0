using Microsoft.Data.Sqlite;
using MyMediaCollection.Enums;
using MyMediaCollection.Interfaces;
using MyMediaCollection.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace MyMediaCollection.Services
{
    public class DataService : IDataService
    {
        private List<MediaItem> _items;
        private IList<ItemType> _itemTypes;
        private IList<Medium> _mediums;
        private IList<LocationType> _locationTypes;
        private const string DbName = "mediaData";

        public int SelectedItemId { get; set; }

        private async Task PopulateItemsAsync(SqliteConnection db)
        {
            _items = await GetAllMediaItemsAsync(db);
        }

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

        public int AddItem(MediaItem item)
        {
            Task<SqliteConnection> connectionTask = GetConnectionAsync();
            connectionTask.Wait();

            using (var db = connectionTask.Result)
            {
                db.Open();

                item.Id = Task.Run(async () => { return await InsertMediaItemAsync(db, item); }).Result;
            }

            _items.Add(item);

            return item.Id;
        }

        public void UpdateItem(MediaItem item)
        {
            Task<SqliteConnection> connectionTask = GetConnectionAsync();
            connectionTask.Wait();

            using (var db = connectionTask.Result)
            {
                db.Open();

                UpdateMediaItemAsync(db, item).Wait();
            }

            _items[_items.FindIndex(ind => ind.Equals(item))] = item;
        }

        public void DeleteItem(MediaItem item)
        {
            Task<SqliteConnection> connectionTask = GetConnectionAsync();
            connectionTask.Wait();

            using (var db = connectionTask.Result)
            {
                db.Open();

                DeleteMediaItemAsync(db, item.Id).Wait();
            }

            _items.Remove(_items.FirstOrDefault(i => i.Id == item.Id));
        }

        public MediaItem GetItem(int id)
        {
            return _items.FirstOrDefault(i => i.Id == id);
        }

        public IList<MediaItem> GetItems()
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
            return _mediums.Where(m => m.MediaType == itemType).ToList();
        }

        public IList<LocationType> GetLocationTypes()
        {
            return _locationTypes;
        }

        public Medium GetMedium(string name)
        {
            return _mediums.FirstOrDefault(m => m.Name == name);
        }

        private Medium GetMedium(int id)
        {
            return _mediums.FirstOrDefault(m => m.Id == id);
        }

        public async Task InitializeDataAsync()
        {
            using (var db = await GetConnectionAsync())
            {
                db.Open();

                await CreateMediumTableAsync(db);
                await CreateMediaItemTableAsync(db);

                SelectedItemId = -1;
                PopulateItemTypes();
                await PopulateMediumsAsync(db);
                PopulateLocationTypes();
                await PopulateItemsAsync(db);

                db.Close();
            }
        }

        private async Task<SqliteConnection> GetConnectionAsync()
        {
            await ApplicationData.Current.RoamingFolder.CreateFileAsync(DbName, CreationCollisionOption.OpenIfExists);
            string dbPath = Path.Combine(ApplicationData.Current.RoamingFolder.Path, DbName);

            return new SqliteConnection($"Filename={dbPath}");
        }

        private async Task CreateMediumTableAsync(SqliteConnection db)
        {
            string tableCommand = "CREATE TABLE IF NOT " +
                "EXISTS Mediums (Id INTEGER PRIMARY KEY, " +
                "Name NVARCHAR(2048), " +
                "MediumType INTEGER)";

            SqliteCommand createTable = new SqliteCommand(tableCommand, db);

            await createTable.ExecuteNonQueryAsync();
        }

        private async Task CreateMediaItemTableAsync(SqliteConnection db)
        {
            string tableCommand = "CREATE TABLE IF NOT " +
                "EXISTS MediaItems (Id INTEGER PRIMARY KEY, " +
                "Name NVARCHAR(2048), " +
                "ItemType INTEGER, " +
                "MediumId INTEGER, " +
                "LocationType INTEGER, " +
                "CONSTRAINT fk_mediums " +
                "FOREIGN KEY(MediumId) " +
                "REFERENCES Mediums(Id))";

            SqliteCommand createTable = new SqliteCommand(tableCommand, db);

            await createTable.ExecuteNonQueryAsync();
        }

        private async Task InsertMediumAsync(SqliteConnection db, Medium medium)
        {
            SqliteCommand insertCommand = new SqliteCommand
            {
                Connection = db,
                CommandText = "INSERT INTO Mediums VALUES (NULL, @Name, @MediumType);"
            };

            insertCommand.Parameters.AddWithValue("@Name", medium.Name);
            insertCommand.Parameters.AddWithValue("@MediumType", (int)medium.MediaType);

            await insertCommand.ExecuteNonQueryAsync();
        }

        private async Task<IList<Medium>> GetAllMediumsAsync(SqliteConnection db)
        {
            IList<Medium> mediums = new List<Medium>();
            var selectCommand = new SqliteCommand("SELECT Id, Name, MediumType from Mediums", db);
            SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

            while (query.Read())
            {
                var medium = new Medium
                {
                    Id = query.GetInt32(0),
                    Name = query.GetString(1),
                    MediaType = (ItemType)query.GetInt32(2)
                };

                mediums.Add(medium);
            }

            return mediums;
        }

        private async Task<List<MediaItem>> GetAllMediaItemsAsync(SqliteConnection db)
        {
            var items = new List<MediaItem>();
            var selectCommand = new SqliteCommand("SELECT Id, Name, ItemType, MediumId, LocationType from MediaItems", db);
            SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

            while (query.Read())
            {
                var item = new MediaItem
                {
                    Id = query.GetInt32(0),
                    Name = query.GetString(1),
                    MediaType = (ItemType)query.GetInt32(2),
                    MediumInfo = GetMedium(query.GetInt32(3)),
                    Location = (LocationType)query.GetInt32(4)
                };

                items.Add(item);
            }

            return items;
        }

        private async Task<int> InsertMediaItemAsync(SqliteConnection db, MediaItem item)
        {
            SqliteCommand insertCommand = new SqliteCommand
            {
                Connection = db,
                CommandText = "INSERT INTO MediaItems VALUES (NULL, @Name, @ItemType, @MediumId, @LocationType);"
            };

            insertCommand.Parameters.AddWithValue("@Name", item.Name);
            insertCommand.Parameters.AddWithValue("@ItemType", (int)item.MediaType);
            insertCommand.Parameters.AddWithValue("@MediumId", item.MediumInfo.Id);
            insertCommand.Parameters.AddWithValue("@LocationType", (int)item.Location);

            await insertCommand.ExecuteNonQueryAsync();

            SqliteCommand getIdCommand = new SqliteCommand
            {
                Connection = db,
                CommandText = "SELECT last_insert_rowid();"
            };

            return int.Parse((await getIdCommand.ExecuteScalarAsync()).ToString());
        }

        private async Task UpdateMediaItemAsync(SqliteConnection db, MediaItem item)
        {
            SqliteCommand updateCommand = new SqliteCommand
            {
                Connection = db,
                CommandText = "UPDATE MediaItems SET Name = @Name, ItemType = @ItemType, MediumId = @MediumId, LocationType = @LocationType WHERE Id = @Id;"
            };

            updateCommand.Parameters.AddWithValue("@Id", item.Id);
            updateCommand.Parameters.AddWithValue("@Name", item.Name);
            updateCommand.Parameters.AddWithValue("@ItemType", (int)item.MediaType);
            updateCommand.Parameters.AddWithValue("@MediumId", item.MediumInfo.Id);
            updateCommand.Parameters.AddWithValue("@LocationType", (int)item.Location);

            await updateCommand.ExecuteNonQueryAsync();
        }

        private async Task DeleteMediaItemAsync(SqliteConnection db, int id)
        {
            SqliteCommand updateCommand = new SqliteCommand
            {
                Connection = db,
                CommandText = "DELETE FROM MediaItems WHERE Id = @Id;"
            };

            updateCommand.Parameters.AddWithValue("@Id", id);

            await updateCommand.ExecuteNonQueryAsync();
        }
    }
}