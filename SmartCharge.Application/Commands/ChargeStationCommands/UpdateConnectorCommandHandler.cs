﻿using AutoMapper;
using MediatR;
using SmartCharge.Application.Exceptions;
using SmartCharge.Core;
using SmartCharge.Core.Entities;
using SmartCharge.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{

    internal sealed class UpdateConnectorCommandHandler : IRequestHandler<UpdateConnectorCommand, UpdateChargeStationDto>
    {
        private readonly IChargeGroupRepository _groupRepository;

        private readonly IChargeStationRepository _stationRepository;
        private readonly IMapper _mapper;

        public UpdateConnectorCommandHandler(IChargeGroupRepository groupRepository, IChargeStationRepository stationRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _stationRepository = stationRepository;
            _mapper = mapper;
        }

        public async Task<UpdateChargeStationDto> Handle(UpdateConnectorCommand command, CancellationToken cancellationToken)
        {
            var found = await _stationRepository.ExistsAsync(command.ChargeStationId).ConfigureAwait(false);

            if (!found)
            {
                throw new ChargeStationNotFoundException(command.ChargeStationId);
            }


            var chargeStation = await _stationRepository.GetAsyncExtended(command.ChargeStationId).ConfigureAwait(false);


            var result = chargeStation.ChangeConnectorMaxCurrent(command.ConnectorId, command.ConnectorMaxCurrentAmps);

            if (result.IsError)
            {
                return new UpdateChargeStationDto
                {
                    IsError = true,

                    ErrorMessage = "ChargeGroup capacity exceeded. You can unplug these connectors:" + result.Suggestions.ToResultString(),
                    ConnectorsToUnplug = result.Suggestions
                };
            }

            await _stationRepository.UpdateConnectorsAsync(chargeStation).ConfigureAwait(false);
            return _mapper.Map<UpdateChargeStationDto>(chargeStation);
        }
              
    }
}
