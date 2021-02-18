using System;

namespace SmartCharge.Application.Exceptions
{
    public class ChargeStationNotFoundException : AppException
    {
        public override string Code { get; } = "charge_station_not_found";
        public Guid Id { get; }

        public ChargeStationNotFoundException(Guid id) : base($"Charge station with id: {id} not found.")
            => Id = id;
    }
}
