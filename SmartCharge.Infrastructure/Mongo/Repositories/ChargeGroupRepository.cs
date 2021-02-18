using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using SmartCharge.Core.Entities;
using SmartCharge.Core.Repositories;
using SmartCharge.Infrastructure.Mongo.Documents;

namespace SmartCharge.Infrastructure.Mongo.Repositories
{
    internal sealed class ChargeGroupRepository : IChargeGroupRepository
    {
        private readonly IMongoCollection<ChargeGroupDocument> _chargeGroupsDocuments;
        private readonly IMongoCollection<ChargeStationDocument> _chargeStationDocuments;

        public ChargeGroupRepository(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _chargeGroupsDocuments = database.GetCollection<ChargeGroupDocument>(settings.ChargeGroupCollectionName);
            _chargeStationDocuments = database.GetCollection<ChargeStationDocument>(settings.ChargeStationCollectionName);
        }

        public async Task<ChargeGroup> GetAsync(Guid chargeGroupId)
        {
            var result = await _chargeGroupsDocuments.FindAsync(g => g.Id == chargeGroupId.ToString()).ConfigureAwait(false);
            return result.FirstOrDefault()?.AsEntity();
        }

        public async Task<ChargeGroup> GetAsyncExtended(Guid chargeGroupId)
        {
            var groupsCursor = await _chargeGroupsDocuments.FindAsync(g => g.Id == chargeGroupId.ToString()).ConfigureAwait(false);
            var stationsCursor = await _chargeStationDocuments.FindAsync(s => s.ChargeGroupId == chargeGroupId.ToString()).ConfigureAwait(false);
            var groupDocument = groupsCursor.FirstOrDefault();
            return groupDocument?.AsEntityExtended(stationsCursor.ToEnumerable().Select(s => s.AsEntityExtended(groupDocument?.AsEntity())));
        }

        public async Task<bool> ExistsAsync(Guid chargeGroupId)
        {
            var result = await _chargeGroupsDocuments.FindAsync(g => g.Id == chargeGroupId.ToString()).ConfigureAwait(false);
            return result.Any();
        }

        public async Task AddAsync(ChargeGroup chargeGroup)
        {
            await _chargeGroupsDocuments.InsertOneAsync(chargeGroup.AsDocument()).ConfigureAwait(false);
        }

        public async Task UpdateAsync(ChargeGroup chargeGroup)
        {
            await _chargeGroupsDocuments.FindOneAndUpdateAsync(
                    Builders<ChargeGroupDocument>.Filter.Where(rec => rec.Id == chargeGroup.Id.ToString()),
                    Builders<ChargeGroupDocument>.Update
                    .Set(rec => rec.Name, chargeGroup.Name)
                    .Set(rec => rec.CapacityAmps, chargeGroup.CapacityAmps)
                    .Set(rec => rec.ChargeStations, chargeGroup.ChargeStations.Select(c => c.Id.ToString()))
                ).ConfigureAwait(false);
        }

        public async Task<long> DeleteAsync(Guid chargeGroupId)
        {
            var result = await _chargeGroupsDocuments.DeleteOneAsync(g => g.Id == chargeGroupId.ToString()).ConfigureAwait(false);
            return result.DeletedCount;
        }
    }
}