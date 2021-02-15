using System;

namespace SmartCharge.Application.Commands
{

   
    public class AddChargeGroup

    {
        public Guid Id { get; }
        public string Name { get; }

        public decimal Capacity { get; }

        public AddChargeGroup(Guid id, string name, decimal capacity)
            => (Id, Name, Capacity) = 
                    (id == Guid.Empty ? Guid.NewGuid() : id,
                    name ?? string.Empty,
                    capacity);
    }
}
