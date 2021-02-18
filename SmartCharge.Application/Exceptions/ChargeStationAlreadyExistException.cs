using System;

namespace SmartCharge.Application.Exceptions
{
    public class ChargeStationAlreadyExistException : AppException
    {
        public override string Code { get; } = "charge_station_already_exists";
        public Guid Id { get; }

        public ChargeStationAlreadyExistException(Guid id) : base($"Charge station with id: {id} already exists.")
            => Id = id;
    }
}
