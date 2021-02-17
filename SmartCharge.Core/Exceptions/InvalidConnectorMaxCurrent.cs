using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Core.Exceptions
{
    public class InvalidConnectorMaxCurrent : DomainException
    {
        public override string Code { get; } = "invalid_connector_max_current";

        public InvalidConnectorMaxCurrent() : base("Connector max current should be greater than 0.")
        {
        }
    }
}
