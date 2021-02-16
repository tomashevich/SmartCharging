using SmartCharge.Core.Entities;
using System;
using System.Linq;


namespace SmartCharge.Infrastructure.Mongo.Documents
{
    internal static class Extensions
    {
        public static ChargeGroup AsEntity(this ChargeGroupDocument document)
            => new ChargeGroup(new Guid(document.Id), document.Name, document.CapacityAmps
                );

        public static ChargeGroupDocument AsDocument(this ChargeGroup entity)
            => new ChargeGroupDocument
            {
                Id = entity.Id.ToString(),
                Version = entity.Version,
                Name = entity.Name,
                CapacityAmps = entity.CapacityAmps
                //Reservations = entity.Reservations.Select(r => new ChargeStationDocument
                //{
                //    TimeStamp = r.DateTime.AsDaysSinceEpoch(),
                //    Priority = r.Priority
                //})
            };

        //    public static ResourceDto AsDto(this ChargeGroupDocument document)
        //        => new ResourceDto
        //        {
        //            Id = document.Id,
        //            Tags = document.Tags ?? Enumerable.Empty<string>(),
        //            Reservations = document.Reservations?.Select(r => new ReservationDto
        //            {
        //                DateTime = r.TimeStamp.AsDateTime(),
        //                Priority = r.Priority
        //            }) ?? Enumerable.Empty<ReservationDto>()
        //        };

        //    internal static int AsDaysSinceEpoch(this DateTime dateTime)
        //        => (dateTime - new DateTime()).Days;

        //    internal static DateTime AsDateTime(this int daysSinceEpoch)
        //        => new DateTime().AddDays(daysSinceEpoch);
    }
}