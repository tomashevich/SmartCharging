using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using SmartCharging.Persistence;
using SmartCharge.Core.Entities;
using SmartCharge.Core.Repositories;
using SmartCharge.Infrastructure.Mongo.Documents;
using SmartCharge.Infrastructure.Mongo.Repositories.Persistence;

namespace SmartCharge.Infrastructure.Mongo.Repositories
{
    public sealed class ChargeGroupRepository : IChargeGroupRepository
    {
        private readonly IMongoCollection<ChargeGroupDocument> _chargeGroupsDocuments;
        private readonly IMongoCollection<ChargeStationDocument> _chargeStationDocuments;
        private readonly IMongoDbContext _dbContext;
        public ChargeGroupRepository(IMongoDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _chargeGroupsDocuments = _dbContext.ChargeGroupsAsync();
            _chargeStationDocuments = _dbContext.ChargeStationsAsync();
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