using MediatR;
using SmartCharge.Application.Exceptions;
using SmartCharge.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCharge.Application.Commands.ChargeGroupCommands
{
    internal sealed class DeleteChargeGroupCommandHandler : IRequestHandler<DeleteChargeGroupCommand, bool>
    {
        private readonly IChargeGroupRepository _groupRepository;
        private readonly IChargeStationRepository _stationRepository;

        public DeleteChargeGroupCommandHandler(IChargeStationRepository stationRepository,
            IChargeGroupRepository groupRepository)
        {
            _stationRepository = stationRepository;
            _groupRepository = groupRepository;
        }

        public async Task<bool> Handle(DeleteChargeGroupCommand command, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetAsyncExtended(command.Id).ConfigureAwait(false);
            if (group == null)
            {
                throw new ChargeGroupNotFoundException(command.Id);
            }

            foreach (var station in group.ChargeStations)
            {
                await _stationRepository.DeleteAsync(station.Id).ConfigureAwait(false);
            }
            await _groupRepository.DeleteAsync(command.Id).ConfigureAwait(false);
            return true;
        }
    }
}
