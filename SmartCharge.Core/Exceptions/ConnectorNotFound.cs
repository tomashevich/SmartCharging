using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Core.Exceptions
{
    class ConnectorNotFound : DomainException
    {
        public override string Code { get; } = "invalid_connector_id";

        public ConnectorNotFound(int id, Guid stationId) : base($"Can not find Connector with id {id} in station with id {stationId}")
        {
        }
    }
}
