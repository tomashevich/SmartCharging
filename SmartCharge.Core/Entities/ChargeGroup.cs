using SmartCharge.Core.Events;
using SmartCharge.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartCharge.Core.Entities
{
    public class ChargeGroup : AggregateRoot
    {
        private ISet<ChargeStation> _chargeStations = new HashSet<ChargeStation>();

        public string Name { get; set; }

        public decimal CapacityAmps { get; set; }

        public decimal CapacityReserve
        {
            get
            {
                return CapacityAmps - _chargeStations.Aggregate(0m, (total, next) => total + next.MaxCurrentAmps, total => total);
            }
        }

    

        public IEnumerable<ChargeStation> ChargeStations
        {
            get => _chargeStations;
            private set => _chargeStations = new HashSet<ChargeStation>(value);
        }

        public ChargeGroup(Guid id, string name, decimal capacityAmps, IEnumerable<ChargeStation> chargeStations)
        {
            Id = IdGuard(id);
            Name = NameGuard(name);
            CapacityAmps = CapacityGuard(capacityAmps);
            ChargeStations = chargeStations ?? new List<ChargeStation>();

        }

        public static ChargeGroup Create(Guid id, string name, decimal capacityAmps, IEnumerable<ChargeStation> chargeStations)
        {
            var chargeGroup = new ChargeGroup(id, name, capacityAmps, chargeStations);
            chargeGroup.AddEvent(new ChargeGroupCreated(chargeGroup));
            return chargeGroup;
        }

        public OperationResult Update(string name, decimal capacityAmps)
        {
            var result = new OperationResult { IsError = false };
            Name = NameGuard(name);

            if (capacityAmps != CapacityAmps)
            {
                result = UpdateCapacity(capacityAmps);
            }

            AddEvent(new ChargeGroupUpdated(this));
            return result;
        }


        public OperationResult UpdateCapacity(decimal newCapacityAmps)
        {
            //todo: check if possible in case of reducing
            var capacityReducedOnAmps = CapacityAmps - newCapacityAmps;

            var needCheck = capacityReducedOnAmps > 0;
            if (needCheck)
            {
                if (CapacityReserve > capacityReducedOnAmps)
                {
                    CapacityAmps = CapacityGuard(newCapacityAmps);
                    return new OperationResult
                    {
                        IsError = false
                    };
                }
                else
                {
                    return new OperationResult
                    {
                        IsError = true,
                        Suggestions = new Algo().FindOptions(this, capacityReducedOnAmps - CapacityReserve)
                    };
                }
            }

            CapacityAmps = CapacityGuard(newCapacityAmps);
            return new OperationResult
            {
                IsError = false
            };
        }

        public OperationResult AddChargeStation(ChargeStation chargeStation)
        {
            if (CapacityReserve > chargeStation.MaxCurrentAmps)
            {
                _chargeStations.Add(chargeStation);
                chargeStation.ParentChargeGroup = this;
                AddEvent(new ChargeGroupUpdated(this));

                return new OperationResult
                {
                    IsError = false
                };
            }
            else
            {
                return new OperationResult
                {
                    IsError = true,
                    Suggestions = new Algo().FindOptions(this, chargeStation.MaxCurrentAmps - CapacityReserve)
                };
            }
        }
        public void RemoveChargeStation(Guid chargeStationId)
        {

            var toRemove = _chargeStations.First(x => x.Id == chargeStationId);
            _chargeStations.Remove(toRemove);

            AddEvent(new ChargeGroupUpdated(this));

        }

        public void Delete()
        {
            foreach (var station in _chargeStations)
            {
                station.Delete();
            }
            AddEvent(new ChargeGroupDeleted(this));
        }

        private static Guid IdGuard(Guid id)
        {
            return id == Guid.Empty ? Guid.NewGuid() : id;
        }

        private static string NameGuard(string name)
        {
            return string.IsNullOrWhiteSpace(name) ? string.Empty : name;
        }

        private static decimal CapacityGuard(decimal capacityAmps)
        {
            if (capacityAmps <= 0)
            {
                throw new InvalidChargeGroupCapacity();
            }
            return capacityAmps;
        }
    }
}
