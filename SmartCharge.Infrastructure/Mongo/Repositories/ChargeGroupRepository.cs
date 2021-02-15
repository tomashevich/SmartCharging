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

        public Task<ChargeGroup> GetAsync(Guid chargeGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Guid chargeGroupId)
        {
            throw new NotImplementedException();
        }

        public void Add(ChargeGroup chargeGroup)
        {

            _documents.InsertOne(chargeGroup.AsDocument());
           
        }

        public Task UpdateAsync(ChargeGroup chargeGroup)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid chargeGroupId)
        {
            throw new NotImplementedException();
        }

        //public async Task<Resource> GetAsync(AggregateId id)
        //{
        //    var document = await _repository.GetAsync(r => r.Id == id);
        //    return document?.AsEntity();
        //}

        //public Task<bool> ExistsAsync(AggregateId id)
        //    => _repository.ExistsAsync(r => r.Id == id);

        //public Task AddAsync(Resource resource)
        //    => _repository.AddAsync(resource.AsDocument());

        //public Task UpdateAsync(Resource resource)
        //    => _repository.Collection.ReplaceOneAsync(r => r.Id == resource.Id && r.Version < resource.Version,
        //        resource.AsDocument());

        //public Task DeleteAsync(AggregateId id)
        //    => _repository.DeleteAsync(id);
    }
}