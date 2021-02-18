using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Core.Events
{
    public class ChargeStationDeleted : IDomainEvent
    {
        public ChargeStation ChargeStation { get;}

        public ChargeStationDeleted(ChargeStation chargeStation) => ChargeStation = chargeStation;
    }
}
