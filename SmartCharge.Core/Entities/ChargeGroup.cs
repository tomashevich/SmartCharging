using SmartCharge.Core.Events;
using SmartCharge.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCharge.Core.Entities
{
    public class ChargeGroup : AggregateRoot
    {
        private ISet<ChargeStation> _chargeStations = new HashSet<ChargeStation>();

        public string Name { get; set; }

        public decimal CapacityAmps { get; set; }

        public decimal CapacityReserve { get; private set; }

        private decimal GetCapacityReserve() => CapacityAmps - _chargeStations.Aggregate(0m, (total, next) => total + next.MaxCurrentAmps);

        public IEnumerable<ChargeStation> ChargeStations
        {
            get => _chargeStations;
            private set => _chargeStations = new HashSet<ChargeStation>(value);
        }

        public ChargeGroup(Guid id, string name, decimal capacityAmps)
        {
            Id = IdGuard(id);
            Name = NameGuard(name);
            CapacityAmps = CapacityGuard(capacityAmps);
            ChargeStations = Enumerable.Empty<ChargeStation>();
        }

        public static ChargeGroup Create(Guid id, string name, decimal capacityAmps)
        {
            var chargeGroup = new ChargeGroup(id, name, capacityAmps);
            chargeGroup.AddEvent(new ChargeGroupCreated(chargeGroup));
            return chargeGroup;
        }

        public void Update(string name, decimal capacityAmps)
        {
            Name = NameGuard(name);

            if (capacityAmps != CapacityAmps)
            {
                UpdateCapacity(capacityAmps);
            }

            AddEvent(new ChargeGroupUpdated(this));
                       
        }

      
        public void UpdateCapacity(decimal capacityAmps)
        {
            //todo: check if possible in case of reducing
            CapacityAmps = CapacityGuard(capacityAmps);
            
        }

        public void AddChargeStation(ChargeStation chargeStation)
        {
            //todo: check capacity
            CapacityReserve = GetCapacityReserve();
            if (CapacityReserve > chargeStation.MaxCurrentAmps)
            {
                _chargeStations.Add(chargeStation);
                CapacityReserve = GetCapacityReserve();
                AddEvent(new ChargeGroupUpdated(this));
            }
            else
            {
                throw new ChargeGroupCapacityExceeded();
            }
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
