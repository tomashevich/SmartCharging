using System;
using System.Threading.Tasks;

using MongoDB.Driver;
using SmartCharge.Core.Entities;
using SmartCharge.Core.Repositories;
using SmartCharge.Infrastructure.Mongo;
using SmartCharge.Infrastructure.Mongo.Documents;

namespace SmartCharge.Infrastructure.Mongo.Repositories
{
    internal sealed class ChargeGroupRepository : IChargeGroupRepository
    {

        private readonly IMongoCollection<ChargeGroupDocument> _documents;

        public ChargeGroupRepository(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _documents = database.GetCollection<ChargeGroupDocument>(settings.CollectionName);
        }

        public async Task<ChargeGroup> GetAsync(Guid chargeGroupId)
        {
            var result = await _documents.FindAsync(x => x.Id == chargeGroupId.ToString()).ConfigureAwait(false);
            return result.FirstOrDefault()?.AsEntity();
        }

        public async Task<bool> ExistsAsync(Guid chargeGroupId)
        {
            var result = await _documents.FindAsync(rec => rec.Id == chargeGroupId.ToString()).ConfigureAwait(false);
            return result.Any();
        }

        public async Task AddAsync(ChargeGroup chargeGroup)
        {

           await  _documents.InsertOneAsync(chargeGroup.AsDocument()).ConfigureAwait(false);
           
        }

        public async Task UpdateAsync(ChargeGroup chargeGroup)
        {
            await _documents.FindOneAndUpdateAsync(
                    Builders<ChargeGroupDocument>.Filter.Where(rec => rec.Id == chargeGroup.Id.ToString()),
                    Builders<ChargeGroupDocument>.Update
                    .Set(rec => rec.Name, chargeGroup.Name)
                    .Set(rec => rec.CapacityAmps, chargeGroup.CapacityAmps),
                    options: new FindOneAndUpdateOptions<ChargeGroupDocument>
                    {
                        ReturnDocument = ReturnDocument.After
                    }
                ).ConfigureAwait(false);
        }

        public async Task <long> DeleteAsync(Guid chargeGroupId)
        {
            var result = await _documents.DeleteOneAsync(x => x.Id == chargeGroupId.ToString()).ConfigureAwait(false);
            return result.DeletedCount;
        }





        //public Task UpdateAsync(Resource resource)
        //    => _repository.Collection.ReplaceOneAsync(r => r.Id == resource.Id && r.Version < resource.Version,
        //        resource.AsDocument());

        //public Task DeleteAsync(AggregateId id)
        //    => _repository.DeleteAsync(id);
    }
}