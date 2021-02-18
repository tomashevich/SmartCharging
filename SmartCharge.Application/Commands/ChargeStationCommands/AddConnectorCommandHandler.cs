using AutoMapper;
using MediatR;
using SmartCharge.Application.Exceptions;
using SmartCharge.Core;
using SmartCharge.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{
    internal sealed class AddConnectorCommandHandler : IRequestHandler<AddConnectorCommand, UpdateChargeStationDto>
    {
        private readonly IChargeStationRepository _stationRepository;
        private readonly IMapper _mapper;

        public AddConnectorCommandHandler(IChargeStationRepository stationRepository, IMapper mapper)
        {
            _stationRepository = stationRepository;
            _mapper = mapper;
        }

        public async Task<UpdateChargeStationDto> Handle(AddConnectorCommand command, CancellationToken cancellationToken)
        {
            var found = await _stationRepository.ExistsAsync(command.ChargeStationId).ConfigureAwait(false);
            if (!found)
            {
                throw new ChargeStationNotFoundException(command.ChargeStationId);
            }

            var chargeStation = await _stationRepository.GetAsyncExtended(command.ChargeStationId).ConfigureAwait(false);
            var result = chargeStation.AddConnector(command.ConnectorMaxCurrentAmps, command.ConnectorId);
            if (result.IsError)
            {
                return new UpdateChargeStationDto
                {
                    IsError = true,
                    ErrorMessage = "ChargeGroup capacity exceeded. You can unplug these connectors:",
                    ConnectorsToUnplug = result.Suggestions.ToResultStrings()
                };
            }

            await _stationRepository.UpdateConnectorsAsync(chargeStation).ConfigureAwait(false);

            return _mapper.Map<UpdateChargeStationDto>(chargeStation);
        }
    }
}
