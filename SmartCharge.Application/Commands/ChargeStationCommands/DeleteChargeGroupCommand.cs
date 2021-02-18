using MediatR;
using System;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{
    public class DeleteChargeStationCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
