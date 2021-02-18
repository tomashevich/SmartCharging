using System.Collections.Generic;

namespace SmartCharge.Core
{
    public class OperationResult
    {
        public bool IsError { get; set; }
        public List<List<ConnectorToUnplug>> Suggestions { get; set; }
    }
}
