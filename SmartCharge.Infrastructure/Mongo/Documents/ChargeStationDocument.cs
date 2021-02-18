using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SmartCharge.Infrastructure.Mongo.Documents
{
    internal sealed class ChargeStationDocument
    {
        [BsonId]
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ConnectorDocument> Connectors { get; set; }
        public string ChargeGroupId { get; set; }
    }
}