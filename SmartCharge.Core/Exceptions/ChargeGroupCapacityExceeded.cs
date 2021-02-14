using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Core.Exceptions
{
    class ChargeGroupCapacityExceeded : DomainException
    {
        public override string Code { get; } = "charge_group_capacity_exceeded";

        public ChargeGroupCapacityExceeded() : base("ChargeGroup capacity exceeded")
        {
        }
    }
}
