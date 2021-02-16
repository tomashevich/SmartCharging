using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace SmartCharge.Infrastructure.Mongo.Documents
{
    
    internal sealed class ChargeGroupDocument 
    {
        [BsonId]
       // [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int Version { get; set; }

       // [BsonElement("Name")]
        public string Name { get; set; }

        public decimal CapacityAmps { get; set; }
        public IEnumerable<ChargeStationDocument> ChargeStations { get; set; }
    }
}