using MediatR;
using System;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{
    public class AddChargeStationCommand : IRequest<AddChargeStationDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ConnectorId { get; set; }
        public decimal ConnectorMaxCurrentAmps { get; set; }
        public Guid ChargeGroupId { get; set; }
    }
}
