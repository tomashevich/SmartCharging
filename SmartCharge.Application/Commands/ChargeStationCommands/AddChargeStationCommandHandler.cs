using AutoMapper;
using MediatR;
using SmartCharge.Application.Exceptions;
using SmartCharge.Core;
using SmartCharge.Core.Entities;
using SmartCharge.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{

    internal sealed class AddChargeStationCommandHandler : IRequestHandler<AddChargeStationCommand, AddChargeStationDto>
    {
        private readonly IChargeGroupRepository _groupRepository;

        private readonly IChargeStationRepository _stationRepository;
        private readonly IMapper _mapper;

        public AddChargeStationCommandHandler(IChargeGroupRepository groupRepository, IChargeStationRepository stationRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _stationRepository = stationRepository;
            _mapper = mapper;
        }

        public async Task<AddChargeStationDto> Handle(AddChargeStationCommand command, CancellationToken cancellationToken)
        {
            if (await _stationRepository.ExistsAsync(command.Id).ConfigureAwait(false))
            {
                throw new ChargeStationAlreadyExistException(command.Id);
            }

            //fetch existed stations and pass to entity
            var chargeGroup = await _groupRepository.GetAsyncExtended(command.ChargeGroupId);

            if (chargeGroup == null)
            {
                throw new ChargeGroupNotFoundException(command.ChargeGroupId);
            }

            //create station

            var chargeStation = ChargeStation.Create( command.Id, command.Name,  chargeGroup);

            //add station to group

            chargeGroup.AddChargeStation(chargeStation);
            //add connector to station
            var result = chargeStation.AddConnector(command.ConnectorMaxCurrentAmps, command.ConnectorId);

            if (result.IsError)
            {
                return new AddChargeStationDto
                {
                    IsError = true,
                    ErrorMessage = "ChargeGroup capacity exceeded. You can unplug these connectors:",
                    ConnectorsToUnplug = result.Suggestions.ToResultString()
                };
            }

            //save to db

            await _stationRepository.AddAsync(chargeStation).ConfigureAwait(false);
                await _groupRepository.UpdateAsync(chargeGroup).ConfigureAwait(false);
            //todo: what about revert changes after fail
            

            return _mapper.Map<AddChargeStationDto>(chargeStation);
        }
              
    }
}
