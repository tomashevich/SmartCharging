using AutoMapper;
using MediatR;
using SmartCharge.Application.Exceptions;
using SmartCharge.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{
    internal sealed class UpdateChargeStationNameCommandHandler : IRequestHandler<UpdateChargeStationNameCommand, UpdateChargeStationDto>
    {
        private readonly IChargeStationRepository _chargeStationRepository;
        private readonly IMapper _mapper;

        public UpdateChargeStationNameCommandHandler(IChargeStationRepository chargeStationRepository, IMapper mapper)
        {
            _chargeStationRepository = chargeStationRepository;
            _mapper = mapper;
        }

        public async Task<UpdateChargeStationDto> Handle(UpdateChargeStationNameCommand command, CancellationToken cancellationToken)
        {
            var found = await _chargeStationRepository.ExistsAsync(command.Id).ConfigureAwait(false);
            if (!found)
            {
                throw new ChargeStationNotFoundException(command.Id);
            }
           
            await _chargeStationRepository.UpdateNameAsync(command.Id, command.Name).ConfigureAwait(false);

            var resource = await _chargeStationRepository.GetAsync(command.Id);
            return _mapper.Map<UpdateChargeStationDto>(resource);
        }              
    }
}
