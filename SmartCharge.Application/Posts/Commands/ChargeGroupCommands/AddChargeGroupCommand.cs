using MediatR;
using SmartCharge.Application.Posts.Commands.ChargeGroupCommands;
using System;

namespace SmartCharge.Application.Commands.ChargeGroupCommands
{

   
    public class AddChargeGroupCommand : IRequest<AddChargeGroupDto>

    {
        public Guid Id { get; }
        public string Name { get; }

        public decimal Capacity { get; }

        //public AddChargeGroupCommand(Guid id, string name, decimal capacity)
        //    => (Id, Name, Capacity) = 
        //            (id == Guid.Empty ? Guid.NewGuid() : id,
        //            name ?? string.Empty,
        //            capacity);
    }
}
