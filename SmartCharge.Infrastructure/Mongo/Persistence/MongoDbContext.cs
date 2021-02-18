using System;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

using SmartCharge.Infrastructure.Mongo;
using SmartCharge.Infrastructure.Mongo.Documents;

namespace SmartCharge.Infrastructure.Mongo.Repositories.Persistence
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly IDatabaseSettings _settings;

        static MongoDbContext()
        {
            BsonClassMap.RegisterClassMap<ChargeGroupDocument>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<ChargeStationDocument>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }

        public MongoDbContext(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
            _settings = settings;
        }

        public  IMongoCollection<ChargeGroupDocument> ChargeGroupsAsync()
        {
            var coll = _database.GetCollection<ChargeGroupDocument>(_settings.ChargeGroupCollectionName);
            return coll;
        }

        public IMongoCollection<ChargeStationDocument> ChargeStationsAsync()
        {
            var coll = _database.GetCollection<ChargeStationDocument>(_settings.ChargeStationCollectionName);
            return coll;
        }      
    }
}