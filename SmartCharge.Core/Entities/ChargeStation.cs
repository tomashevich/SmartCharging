using SmartCharge.Core.Events;
using SmartCharge.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCharge.Core.Entities
{

    public class ChargeStation : AggregateRoot
    {


        private ISet<Connector> _connectors = new HashSet<Connector>();

        public decimal MaxCurrentAmps { get
            {
              return  _connectors.Aggregate(0m, (total, next) => total + next.MaxCurrentAmps, total=>total);
            } }



        public string Name { get; set; }

        public ChargeGroup ParentChargeGroup { get; set; }


        public IEnumerable<Connector> Connectors
        {
            get => _connectors;
            set => _connectors = new HashSet<Connector>(value);
        }

        public ChargeStation(Guid id, string name, ChargeGroup parentChargeGroup)
        {

            Id = IdGuard(id);
            Name = NameGuard(name);
            Connectors = Enumerable.Empty<Connector>();
            ParentChargeGroup = parentChargeGroup;
        }

       

        public static ChargeStation Create(Guid id, string name, ChargeGroup parentChargeGroup)
        {
            var chargeStation = new ChargeStation(id, name, parentChargeGroup);
            chargeStation.AddEvent(new ChargeStationCreated(chargeStation));
            //chargeStation.AddConnector(connectorMaxCurrent);
            return chargeStation;
        }


        public void UpdateName(string name)
        {
            Name = NameGuard(name);
            AddEvent(new ChargeStationUpdated(this));
        }

        public void AddConnector(decimal connectorMaxCurrent, int connectorId = 0)
        {

            if (ParentChargeGroup.CapacityReserve >= connectorMaxCurrent)
            {
                int id = GetNewConnectorId(connectorId);
                if (_connectors.Count() < Const.MAX_CONNECTORS)
                {
                    _connectors.Add(new Connector(id, connectorMaxCurrent, Id));
                   
                   
                    AddEvent(new ChargeStationUpdated(this));
                }
                else
                {
                    throw new InvalidChargeStationConnectorsAmount();
                }
            }
            else
            {
                throw new ChargeGroupCapacityExceeded();
            }
        }
        public void ChangeConnectorMaxCurrent(int connectorId, decimal newMaxCurrentAmps)
        {

            var connectorUnderChange = _connectors.FirstOrDefault(x => x.Id == connectorId);
            if (connectorUnderChange == null)
            {
                throw new ConnectorNotFound(connectorId, Id);
            }

            var maxCurrentDelta = newMaxCurrentAmps - connectorUnderChange.MaxCurrentAmps;
            if (ParentChargeGroup.CapacityReserve > maxCurrentDelta)
            {
                connectorUnderChange.ChangeConnectorMaxCurrent(newMaxCurrentAmps);
                //MaxCurrentAmps = GetMaxCurrentAmps();
                AddEvent(new ChargeStationUpdated(this));
            }
            else { throw new ChargeGroupCapacityExceeded(); }
        }

        private int GetNewConnectorId(int connectorId)
        {
            if (connectorId == 0)
            {

                for (int i = 1; i <= Const.MAX_CONNECTOR_ID_VALUE; i++)
                {
                    bool freeId = !_connectors.Any(x => x.Id == i);
                    if (freeId)
                    {
                        return i;
                    }
                }
            }
            else 
            {
                if (!_connectors.Any(x => x.Id == connectorId))
                {
                    return connectorId;
                }
            }

            throw new InvalidConnectorId();
        }



        public void Delete()
        {

            AddEvent(new ChargeStationDeleted(this));
        }

        private static Guid IdGuard(Guid id)
        {
            return id == Guid.Empty ? Guid.NewGuid() : id;
        }

        private static string NameGuard(string name)
        {
            return string.IsNullOrWhiteSpace(name) ? string.Empty : name;
        }

    }
}
