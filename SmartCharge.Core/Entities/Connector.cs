using SmartCharge.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Core.Entities
{
    public class Connector
    {
        
        public int Id { get; set; }
        //public int MaxCurrentMilliAmps { get; set; }
        public decimal MaxCurrentAmps { get; set; }
        public Guid ParentChargeStationId { get; }

        public Connector(int id, decimal maxCurrentAmps, Guid parentChargeStationId)
        {
            Id = GuardId(id);
            MaxCurrentAmps = GuardMaxCurrrent(maxCurrentAmps);
            ParentChargeStationId = parentChargeStationId;
        }

        public void ChangeConnectorMaxCurrent(decimal maxCurrentAmps)
        {
            MaxCurrentAmps = GuardMaxCurrrent(maxCurrentAmps);
        }

        private int GuardId(int id)
        {
            if (id < Const.MIN_CONNECTOR_ID_VALUE || id > Const.MAX_CONNECTOR_ID_VALUE)
            {
                throw new InvalidConnectorId();
            }
            return id;
        }

        private static decimal GuardMaxCurrrent(decimal maxCurrentAmps)
        {
            if (maxCurrentAmps <= 0)
            {
                throw new InvalidConnectorMaxCurrent();
            }
            return maxCurrentAmps;
        }
    }
}
