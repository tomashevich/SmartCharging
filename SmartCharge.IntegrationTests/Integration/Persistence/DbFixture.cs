using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SmartCharging.Persistence;
using SmartCharge.Infrastructure.Mongo;
using SmartCharge.Infrastructure.Mongo.Repositories.Persistence;

namespace SmartCharging.IntegrationTests.Integration.Persistence
{
    public class DbFixture : IDisposable
    {
        public DbFixture()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connString = config.GetConnectionString("db");
            var dbName = $"test_db_{Guid.NewGuid()}";

            this.DbContextSettings = new DatabaseSettings() {
                DatabaseName = dbName,
                ConnectionString = connString,
                ChargeStationCollectionName = "ChargeStationTest",
                ChargeGroupCollectionName = "ChargeGroupTest"
            };
            this.DbContext = new MongoDbContext(this.DbContextSettings);
        }

        public DatabaseSettings DbContextSettings { get; }
        public MongoDbContext DbContext { get; }

        public void Dispose()
        {
            var client = new MongoClient(this.DbContextSettings.ConnectionString);
            client.DropDatabase(this.DbContextSettings.DatabaseName);
        }
    }
}