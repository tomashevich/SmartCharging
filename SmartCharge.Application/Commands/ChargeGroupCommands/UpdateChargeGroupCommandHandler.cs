using AutoMapper;
using MediatR;
using SmartCharge.Application.Exceptions;
using SmartCharge.Core;
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
            var result = resource.Update( command.Name, command.Capacity);
            if (result.IsError)
            {
                return new UpdateChargeGroupDto
                {
                    IsError = true,
                    ErrorMessage = "ChargeGroup capacity exceeded. You can unplug these connectors:",
                    ConnectorsToUnplug = result.Suggestions.ToResultString()
                };
            }
            await _repository.UpdateAsync(resource).ConfigureAwait(false);
            return _mapper.Map<UpdateChargeGroupDto>(resource);
        }
              
    }
}
