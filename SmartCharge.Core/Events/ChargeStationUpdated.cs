using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Core.Events
{
    public class ChargeStationUpdated : IDomainEvent
    {
        public ChargeStation ChargeStation { get;}

        public ChargeStationUpdated(ChargeStation chargeStation) => ChargeStation = chargeStation;
    }
}
