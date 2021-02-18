using MediatR;
using System;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{
    public class GetChargeStationQuery : IRequest<GetChargeStationDto>
    {
        public Guid Id { get; set; }
    }
}
