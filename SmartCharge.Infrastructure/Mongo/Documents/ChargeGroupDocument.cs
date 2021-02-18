using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartCharge.Infrastructure.Mongo.Documents
{
    internal sealed class ChargeGroupDocument
    {
        [BsonId]
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal CapacityAmps { get; set; }
        public IEnumerable<string> ChargeStations { get; set; }
    }
}