using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Core.Events
{
    public class ChargeGroupUpdated : IDomainEvent
    {
        public ChargeGroup ChargeGroup  { get;}

        public ChargeGroupUpdated(ChargeGroup chargeGroup) => ChargeGroup = chargeGroup;
    }
}
