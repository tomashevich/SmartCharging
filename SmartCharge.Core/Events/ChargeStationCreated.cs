using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Core.Events
{
    public class ChargeStationCreated : IDomainEvent
    {
        public ChargeStation ChargeStation { get;}

        public ChargeStationCreated(ChargeStation chargeStation) => ChargeStation = chargeStation;
    }
}
