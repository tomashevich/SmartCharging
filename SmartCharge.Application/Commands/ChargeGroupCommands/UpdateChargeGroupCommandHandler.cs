using AutoMapper;
using MediatR;
using SmartCharge.Application.Exceptions;
using SmartCharge.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCharge.Application.Commands.ChargeGroupCommands
{

    internal sealed class UpdateChargeGroupCommandHandler : IRequestHandler<UpdateChargeGroupCommand, UpdateChargeGroupDto>
    {
        private readonly IChargeGroupRepository _repository;
        private readonly IMapper _mapper;

        public UpdateChargeGroupCommandHandler(IChargeGroupRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UpdateChargeGroupDto> Handle(UpdateChargeGroupCommand command, CancellationToken cancellationToken)
        {
            var resource = await _repository.GetAsyncExtended(command.Id);

            if (resource == null)
            {
                throw new ChargeGroupNotFoundException(command.Id);
            }
            resource.Update( command.Name, command.Capacity);
            await _repository.UpdateAsync(resource).ConfigureAwait(false);
            return _mapper.Map<UpdateChargeGroupDto>(resource);
        }
              
    }
}
