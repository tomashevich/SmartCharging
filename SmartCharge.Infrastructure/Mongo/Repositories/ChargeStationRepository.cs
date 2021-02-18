using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using SmartCharge.Core.Entities;
using SmartCharge.Core.Repositories;
using SmartCharge.Infrastructure.Mongo.Documents;

namespace SmartCharge.Infrastructure.Mongo.Repositories
{
    internal sealed class ChargeStationRepository : IChargeStationRepository
    {
        private readonly IMongoCollection<ChargeStationDocument> _chargeStationDocuments;
        private readonly IMongoCollection<ChargeGroupDocument> _chargeGroupsDocuments;
        public ChargeStationRepository(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _chargeGroupsDocuments = database.GetCollection<ChargeGroupDocument>(settings.ChargeGroupCollectionName);
            _chargeStationDocuments = database.GetCollection<ChargeStationDocument>(settings.ChargeStationCollectionName);
        }

        public async Task AddAsync(ChargeStation chargeStation)
        {
            await _chargeStationDocuments.InsertOneAsync(chargeStation.AsDocument()).ConfigureAwait(false);
        }

        public async Task<ChargeStation> GetAsync(Guid chargeStationId)
        {
            var stationsCursor = await _chargeStationDocuments.FindAsync(s => s.Id == chargeStationId.ToString()).ConfigureAwait(false);
            var stationDocument = stationsCursor.FirstOrDefault();

            if (stationDocument != null)
            {
                var groupsCursor = await _chargeGroupsDocuments.FindAsync(g => g.Id == stationDocument.ChargeGroupId).ConfigureAwait(false);
                var groupDocument = groupsCursor.FirstOrDefault();
                return stationDocument?.AsEntityExtended(groupDocument?.AsEntity());
            }

            return stationDocument?.AsEntity();
        }

        public async Task<ChargeStation> GetAsyncExtended(Guid chargeStationId)
        {
            var stationsCursor = await _chargeStationDocuments.FindAsync(s => s.Id == chargeStationId.ToString()).ConfigureAwait(false);
            var stationToReturnDocument = stationsCursor.FirstOrDefault();

            if (stationToReturnDocument != null)
            {
                var stationsInGroupCursor = await _chargeStationDocuments.FindAsync(
                    s => s.ChargeGroupId == stationToReturnDocument.ChargeGroupId).ConfigureAwait(false);

                var groupsCursor = await _chargeGroupsDocuments.FindAsync(
                    x => x.Id == stationToReturnDocument.ChargeGroupId).ConfigureAwait(false);

                var groupDocument = groupsCursor.FirstOrDefault();

                return stationToReturnDocument?.AsEntityExtended(groupDocument?.AsEntityExtended(
                    stationsInGroupCursor.ToEnumerable().Select(s => s.AsEntityExtended(groupDocument?.AsEntity()))));
            }

            return stationToReturnDocument?.AsEntity();
        }

        public async Task UpdateNameAsync(Guid id, string name)
        {
            await _chargeStationDocuments.FindOneAndUpdateAsync(
                    Builders<ChargeStationDocument>.Filter.Where(s => s.Id == id.ToString()),
                    Builders<ChargeStationDocument>.Update
                    .Set(rec => rec.Name, name)
                ).ConfigureAwait(false);
        }

        public async Task UpdateChargeGroupAsync(Guid id, Guid chargeGroupId)
        {
            await _chargeStationDocuments.FindOneAndUpdateAsync(
                    Builders<ChargeStationDocument>.Filter.Where(s => s.Id == id.ToString()),
                    Builders<ChargeStationDocument>.Update
                    .Set(rec => rec.ChargeGroupId, chargeGroupId.ToString())
                ).ConfigureAwait(false);
        }
        public async Task UpdateConnectorsAsync(ChargeStation chargeStation)
        {
            await _chargeStationDocuments.FindOneAndUpdateAsync(
                    Builders<ChargeStationDocument>.Filter.Where(s => s.Id == chargeStation.Id.ToString()),
                    Builders<ChargeStationDocument>.Update
                    .Set(s => s.Connectors, chargeStation.Connectors.Select(c => c.AsDocument()))
                ).ConfigureAwait(false);
        }

        public async Task<long> DeleteAsync(Guid chargeStationId)
        {
            var result = await _chargeStationDocuments.DeleteOneAsync(s => s.Id == chargeStationId.ToString()).ConfigureAwait(false);
            return result.DeletedCount;
        }

        public async Task<bool> ExistsAsync(Guid chargeStationId)
        {
            var result = await _chargeStationDocuments.FindAsync(s => s.Id == chargeStationId.ToString()).ConfigureAwait(false);
            return result.Any();
        }
    }
}