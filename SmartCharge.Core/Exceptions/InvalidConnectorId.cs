using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Core.Exceptions
{
    public class InvalidConnectorId : DomainException
    {
        public override string Code { get; } = "invalid_connector_id";

        public InvalidConnectorId() : base("Connector id should be a number from 1 to 10.")
        {
        }
    }
}
