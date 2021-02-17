using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Core
{
    public class OperationResult
    {
        public bool IsError { get; set; }
        public List<List<ConnectorToUnplug>> Suggestions { get; set; }
    }
}
