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

    internal sealed class ChangeGroupCommandHandler : IRequestHandler<ChangeGroupCommand, UpdateChargeStationDto>
    {
        private readonly IChargeGroupRepository _groupRepository;

        private readonly IChargeStationRepository _stationRepository;
        private readonly IMapper _mapper;

        public ChangeGroupCommandHandler(IChargeGroupRepository groupRepository, IChargeStationRepository stationRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _stationRepository = stationRepository;
            _mapper = mapper;
        }

        public async Task<UpdateChargeStationDto> Handle(ChangeGroupCommand command, CancellationToken cancellationToken)
        {
            //find station
            var chargeStation = await _stationRepository.GetAsyncExtended(command.ChargeStationId).ConfigureAwait(false);
            if (chargeStation == null)
            {
                throw new ChargeStationNotFoundException(command.ChargeStationId);
            }

            //find new group
            var newGroup = await _groupRepository.GetAsyncExtended(command.ChargeGroupId).ConfigureAwait(false);
            if (newGroup == null)
            {
                throw new ChargeGroupNotFoundException(command.ChargeGroupId);
            }

            var oldGroupId = chargeStation.ParentChargeGroup.Id;
            var result = newGroup.AddChargeStation(chargeStation);

            if (result.IsError)
            {
                return new UpdateChargeStationDto
                {
                    IsError = true,
                    ErrorMessage = "ChargeGroup capacity exceeded. You can unplug these connectors:",
                    ConnectorsToUnplug = result.Suggestions.ToResultString()
                };
            }

            //find old group
            var oldGroup = await _groupRepository.GetAsyncExtended(oldGroupId).ConfigureAwait(false);

            //delete from old group
            if (oldGroup == null)
            {
                throw new ChargeGroupNotFoundException(oldGroupId);
            }

            oldGroup.RemoveChargeStation(chargeStation.Id);

            await _stationRepository.UpdateChargeGroupAsync(chargeStation.Id, chargeStation.ParentChargeGroup.Id).ConfigureAwait(false);
            await _groupRepository.UpdateAsync(newGroup).ConfigureAwait(false);
            await _groupRepository.UpdateAsync(oldGroup).ConfigureAwait(false);

            return _mapper.Map<UpdateChargeStationDto>(chargeStation);
        }
              
    }
}
