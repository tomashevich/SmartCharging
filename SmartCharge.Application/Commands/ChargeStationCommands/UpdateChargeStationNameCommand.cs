using MediatR;
using System;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{
    public class UpdateChargeStationNameCommand : IRequest<UpdateChargeStationDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
