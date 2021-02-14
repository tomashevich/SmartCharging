using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Core.Exceptions
{
    class InvalidChargeStationConnectorsAmount : DomainException
    {
        public override string Code { get; } = "invalid_charge_station_connectors_amount";

        public InvalidChargeStationConnectorsAmount() : 
            base($"Charge station connectors amount should be not greater than {Const.MAX_CONNECTORS}.")
        {
        }
    }
}
