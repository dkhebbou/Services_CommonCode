using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Play.CommonCode.MongoDB
{
    public class MongoRepoitory<T> : IRepository<T> where T : IEntity
    {
        private readonly IMongoCollection<T> dbCollection;
        private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

        public MongoRepoitory(IMongoDatabase database, string collectionName)
        {
            dbCollection = database.GetCollection<T>(collectionName);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid Id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, Id);
            var item = dbCollection.Find(filter).FirstOrDefaultAsync();
            return await item;
        }

        public async Task CreateItemAsync(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            await dbCollection.InsertOneAsync(item);

        }

        public async Task DeleteItemAsync(Guid Id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, Id);
            await dbCollection.DeleteOneAsync(filter);

        }

        public async Task UpdateItemAsync(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            FilterDefinition<T> filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
            await dbCollection.ReplaceOneAsync(filter, item);

        }
    }
}