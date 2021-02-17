using AutoMapper;
using MediatR;
using SmartCharge.Application.Exceptions;
using SmartCharge.Core.Entities;
using SmartCharge.Core.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCharge.Application.Commands.ChargeGroupCommands
{

    internal sealed class AddChargeGroupCommandHandler : IRequestHandler<AddChargeGroupCommand, AddChargeGroupDto>
    {
        private readonly IChargeGroupRepository _repository;
        private readonly IMapper _mapper;

        public AddChargeGroupCommandHandler(IChargeGroupRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AddChargeGroupDto> Handle(AddChargeGroupCommand command, CancellationToken cancellationToken)
        {
            if (await _repository.ExistsAsync(command.Id).ConfigureAwait(false))
            {
                throw new ChargeGroupAlreadyExistException(command.Id);
            }

            var resource = ChargeGroup.Create( command.Id, command.Name, command.Capacity, new List<ChargeStation>());
            await _repository.AddAsync(resource).ConfigureAwait(false);
            return _mapper.Map<AddChargeGroupDto>(resource);
        }
              
    }
}
