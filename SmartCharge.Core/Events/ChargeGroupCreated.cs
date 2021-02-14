using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Core.Events
{
    public class ChargeGroupCreated : IDomainEvent
    {
        public ChargeGroup ChargeGroup  { get;}

        public ChargeGroupCreated(ChargeGroup chargeGroup) => ChargeGroup = chargeGroup;
    }
}
