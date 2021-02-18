using MediatR;
using System;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{
    public class ChangeGroupCommand : IRequest<UpdateChargeStationDto>
    {
        public Guid ChargeStationId { get; set; }
        public Guid ChargeGroupId { get; set; }
    }
}
