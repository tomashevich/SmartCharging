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
        private readonly IChargeStationRepository _repository;
        private readonly IMapper _mapper;

        public UpdateChargeStationNameCommandHandler(IChargeStationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UpdateChargeStationDto> Handle(UpdateChargeStationNameCommand command, CancellationToken cancellationToken)
        {
            var found = await _repository.ExistsAsync(command.Id).ConfigureAwait(false);

            if (!found)
            {
                throw new ChargeStationNotFoundException(command.Id);
            }
           
            await _repository.UpdateNameAsync(command.Id, command.Name).ConfigureAwait(false);

            var resource = await _repository.GetAsync(command.Id);
            return _mapper.Map<UpdateChargeStationDto>(resource);
        }
              
    }
}
