using AutoMapper;
using MediatR;
using SmartCharge.Application.Exceptions;
using SmartCharge.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{

    internal sealed class DeleteChargeStationCommandHandler : IRequestHandler<DeleteChargeStationCommand, bool>
    {
        private readonly IChargeGroupRepository _groupRepository;

        private readonly IChargeStationRepository _stationRepository;
        private readonly IMapper _mapper;

        public DeleteChargeStationCommandHandler(IChargeStationRepository stationRepository, IChargeGroupRepository groupRepository, IMapper mapper)
        {
            _stationRepository = stationRepository;
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteChargeStationCommand command, CancellationToken cancellationToken)
        {

            var stationFound = await _stationRepository.ExistsAsync(command.Id);

            if (!stationFound)
            {
                throw new ChargeStationNotFoundException(command.Id);
            }

            var station = await _stationRepository.GetAsync(command.Id);

            if (station.ParentChargeGroup == null)
            {
                throw new ChargeGroupNotFoundException(command.Id);
            }

          
            var group = await _groupRepository.GetAsyncExtended(station.ParentChargeGroup.Id);

            group.RemoveChargeStation(station.Id);
            await _groupRepository.UpdateAsync(group).ConfigureAwait(false);

            await _stationRepository.DeleteAsync(command.Id).ConfigureAwait(false);
                     
            return true;
        }

      

       
    }
}
