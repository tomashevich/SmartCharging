using MediatR;
using System;

namespace SmartCharge.Application.Commands.ChargeGroupCommands
{
    public class DeleteChargeGroupCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
