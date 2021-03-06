﻿using AutoMapper;
using MediatR;
using SmartCharge.Application.Exceptions;
using SmartCharge.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{
    internal sealed class GetChargeStationQueryHandler : IRequestHandler<GetChargeStationQuery, GetChargeStationDto>
    {
        private readonly IChargeStationRepository _stationRepository;
        private readonly IMapper _mapper;

        public GetChargeStationQueryHandler(IChargeStationRepository stationRepository, IMapper mapper)
        {
            _stationRepository = stationRepository;
            _mapper = mapper;
        }

        public async Task<GetChargeStationDto> Handle(GetChargeStationQuery command, CancellationToken cancellationToken)
        {          
            var chargeStation = await _stationRepository.GetAsync(command.Id).ConfigureAwait(false);
            if (chargeStation == null)
            {
                throw new ChargeStationNotFoundException(command.Id);
            }

            return _mapper.Map<GetChargeStationDto>(chargeStation);
        }
    }
}
