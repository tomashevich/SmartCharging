using MediatR;
using SmartCharge.Application.Posts.Commands.ChargeGroupCommands;
using System;

namespace SmartCharge.Application.Commands.ChargeGroupCommands
{   
    public class AddChargeGroupCommand : IRequest<AddChargeGroupDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Capacity { get; set; }
    }
}
