using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Play.CommonCode.Settings;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Play.CommonCode.MongoDB
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            services.AddSingleton(ServiceProvider =>
            {
                var configuration = ServiceProvider.GetService<IConfiguration>();
                var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                var mongoDbClient = new MongoClient(mongoDbSettings.ConnectionString);
                return mongoDbClient.GetDatabase(serviceSettings.ServiceName);
            });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName)
            where T : IEntity
        {
            var database = services.AddSingleton<IRepository<T>>(serviceProvider =>
            {
                var database = serviceProvider.GetService<IMongoDatabase>();
                return new MongoRepoitory<T>(database, collectionName);
            });

            return services;
        }
    }
}
