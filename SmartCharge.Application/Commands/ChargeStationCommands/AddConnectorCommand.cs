using MediatR;
using System;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{
    public class AddConnectorCommand : IRequest<UpdateChargeStationDto>
    {
     
        public int ConnectorId { get; set; }
        public decimal ConnectorMaxCurrentAmps { get; set; }
        public Guid ChargeStationId { get; set; }
    }
}
