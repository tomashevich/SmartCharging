using MediatR;
using SmartCharge.Application.Posts.Commands.ChargeGroupCommands;
using System;

namespace SmartCharge.Application.Commands.ChargeGroupCommands
{   
    public class DeleteChargeGroupCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
