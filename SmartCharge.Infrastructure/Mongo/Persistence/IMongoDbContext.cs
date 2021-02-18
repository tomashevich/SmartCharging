using System.Threading.Tasks;
using MongoDB.Driver;

using SmartCharge.Infrastructure.Mongo.Documents;

namespace SmartCharge.Infrastructure.Mongo.Repositories.Persistence
{
    public interface IMongoDbContext
    {
        IMongoCollection<ChargeGroupDocument> ChargeGroupsAsync();
        IMongoCollection<ChargeStationDocument> ChargeStationsAsync();
    }
}