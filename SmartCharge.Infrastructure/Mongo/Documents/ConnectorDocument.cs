using MongoDB.Bson.Serialization.Attributes;

namespace SmartCharge.Infrastructure.Mongo.Documents
{
    internal sealed class ConnectorDocument
    {
        
        public int Id { get; set; }
        public decimal MaxCurrentAmps { get; set; }
        public string ParentChargeStationId { get; set; }


    }
}