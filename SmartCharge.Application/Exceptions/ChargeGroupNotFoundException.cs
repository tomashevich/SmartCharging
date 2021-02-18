using System;

namespace SmartCharge.Application.Exceptions
{
    public class ChargeGroupNotFoundException : AppException
    {
        public override string Code { get; } = "charge_group_not_found";
        public Guid Id { get; }

        public ChargeGroupNotFoundException(Guid id) : base($"Charge group with id: {id} not found.")
            => Id = id;
    }
}
