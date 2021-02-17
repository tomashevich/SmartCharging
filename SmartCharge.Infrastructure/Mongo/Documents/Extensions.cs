using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SmartCharge.Infrastructure.Mongo.Documents
{
    internal static class Extensions
    {
        public static ChargeGroup AsEntity(this ChargeGroupDocument document)
        {
            return new ChargeGroup(new Guid(document.Id), document.Name, document.CapacityAmps, 
                          null); 
        }

        public static ChargeGroup AsEntityExtended(this ChargeGroupDocument document, IEnumerable<ChargeStation> stations )
        {
            return new ChargeGroup(new Guid(document.Id), document.Name, document.CapacityAmps,
                           stations);
        }

        public static ChargeGroupDocument AsDocument(this ChargeGroup entity)
            => new ChargeGroupDocument
            {
                Id = entity.Id.ToString(),
  
                Name = entity.Name,
                CapacityAmps = entity.CapacityAmps,
                ChargeStations = entity.ChargeStations.Select(c => c.Id.ToString())
                //Reservations = entity.Reservations.Select(r => new ChargeStationDocument
                //{
                //    TimeStamp = r.DateTime.AsDaysSinceEpoch(),
                //    Priority = r.Priority
                //})
            };

        public static ChargeStation AsEntity(this ChargeStationDocument document)
           => new ChargeStation(
               new Guid(document.Id),
               document.Name,
               null)
           { Connectors = document.Connectors.Select(c=>new Connector( c.Id, c.MaxCurrentAmps , new Guid(c.ParentChargeStationId)))};

        public static ChargeStation AsEntityExtended(this ChargeStationDocument document, ChargeGroup chargeGroup)
     => new ChargeStation(
         new Guid(document.Id),
         document.Name,
         chargeGroup)
     { Connectors = document.Connectors.Select(c => new Connector(c.Id, c.MaxCurrentAmps, new Guid(c.ParentChargeStationId))) };

        public static ChargeStationDocument AsDocument(this ChargeStation entity)
           => new ChargeStationDocument
           {
               Id = entity.Id.ToString(),

               Name = entity.Name,
               ChargeGroupId = entity.ParentChargeGroup.Id.ToString(),
               Connectors = entity.Connectors.Select(c=> c.AsDocument())
             


           };

        public static ConnectorDocument AsDocument(this Connector entity)
           => new ConnectorDocument
           {
               Id = entity.Id,
               MaxCurrentAmps = entity.MaxCurrentAmps,
               ParentChargeStationId = entity.ParentChargeStationId.ToString()
           };

    }
}