using SmartCharge.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Core.Entities
{
    public class Connector
    {
        
        public int Id { get; set; }
        public int MaxCurrentMilliAmps { get; set; }
        public decimal MaxCurrentAmps { get { return MaxCurrentMilliAmps / 1000; } }
        public Guid ParentChargeStationId { get; }

        public Connector(int id, int maxCurrentMilliAmps, Guid parentChargeStationId)
        {
            Id = GuardId(id);
            MaxCurrentMilliAmps = GuardMaxCurrrent(maxCurrentMilliAmps);
            ParentChargeStationId = parentChargeStationId;
        }

        public void ChangeConnectorMaxCurrent(int maxCurrentMilliAmps)
        {
            MaxCurrentMilliAmps = GuardMaxCurrrent(maxCurrentMilliAmps);
        }

        private int GuardId(int id)
        {
            if (id < Const.MIN_CONNECTOR_ID_VALUE || id > Const.MAX_CONNECTOR_ID_VALUE)
            {
                throw new InvalidConnectorId();
            }
            return id;
        }

        private static int GuardMaxCurrrent(int maxCurrentMilliAmps)
        {
            if (maxCurrentMilliAmps <= 0)
            {
                throw new InvalidConnectorMaxCurrent();
            }
            return maxCurrentMilliAmps;
        }
    }
}
