using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Core.Events
{
    public class ChargeGroupDeleted: IDomainEvent
    {
        public ChargeGroup ChargeGroup { get; }

        public ChargeGroupDeleted(ChargeGroup chargeGroup) => ChargeGroup = chargeGroup;
    }
}
