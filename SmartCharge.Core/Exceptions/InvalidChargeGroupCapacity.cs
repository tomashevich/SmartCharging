using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Core.Exceptions
{
    public class InvalidChargeGroupCapacity : DomainException
    {
        public override string Code { get; } = "invalid_charge_group_capacity";

        public InvalidChargeGroupCapacity() : base("ChargeGroup capacity should be greater than 0.")
        {
        }
    }
}
