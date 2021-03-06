﻿using SmartCharge.Core.Events;
using SmartCharge.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartCharge.Core.Entities
{
    public class ChargeStation : AggregateRoot
    {
        private ISet<Connector> _connectors = new HashSet<Connector>();
        public string Name { get; set; }
        public ChargeGroup ParentChargeGroup { get; set; }

        public IEnumerable<Connector> Connectors
        {
            get => _connectors;
            set => _connectors = new HashSet<Connector>(value);
        }

        public decimal MaxCurrentAmps
        {
            get
            {
                return _connectors.Aggregate(0m, (total, next) => total + next.MaxCurrentAmps, total => total);
            }
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
            return chargeStation;
        }

        public void UpdateName(string name)
        {
            Name = NameGuard(name);
            AddEvent(new ChargeStationUpdated(this));
        }

        public OperationResult AddConnector(decimal connectorMaxCurrent, int connectorId = 0)
        {
            var reserve = ParentChargeGroup.CapacityReserve;
            if (reserve >= connectorMaxCurrent)
            {
                int id = GetNewConnectorId(connectorId);
                if (_connectors.Count() < Const.MAX_CONNECTORS)
                {
                    _connectors.Add(new Connector(id, connectorMaxCurrent, Id));

                    AddEvent(new ChargeStationUpdated(this));
                    return Success();
                }
                else
                {
                    throw new InvalidChargeStationConnectorsAmount();
                }
            }
            else
            {
                return FindSuggestions(connectorMaxCurrent - reserve);
            }
        }

        public OperationResult ChangeConnectorMaxCurrent(int connectorId, decimal newMaxCurrentAmps)
        {
            var connectorUnderChange = _connectors.FirstOrDefault(x => x.Id == connectorId);
            if (connectorUnderChange == null)
            {
                throw new ConnectorNotFound(connectorId, Id);
            }

            var maxCurrentDelta = newMaxCurrentAmps - connectorUnderChange.MaxCurrentAmps;
            var reserve = ParentChargeGroup.CapacityReserve;
            if (reserve > maxCurrentDelta)
            {
                connectorUnderChange.ChangeConnectorMaxCurrent(newMaxCurrentAmps);
                AddEvent(new ChargeStationUpdated(this));
                return Success();
            }
            else
            {
                return FindSuggestions(maxCurrentDelta - reserve);                
            }
        }

        public void Delete()
        {
            AddEvent(new ChargeStationDeleted(this));
        }

        private OperationResult FindSuggestions(decimal needToFree)
        {
            return new OperationResult
            {
                IsError = true,
                Suggestions = new Algo().FindOptions(ParentChargeGroup, needToFree)
            };
        }

        private static OperationResult Success()
        {
            return new OperationResult
            {
                IsError = false
            };
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
