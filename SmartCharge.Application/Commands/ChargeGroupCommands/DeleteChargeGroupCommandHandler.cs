using AutoMapper;
using MediatR;
using SmartCharge.Application.Exceptions;
using SmartCharge.Application.Posts.Commands.ChargeGroupCommands;
using SmartCharge.Core.Entities;
using SmartCharge.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCharge.Application.Commands.ChargeGroupCommands
{

    internal sealed class DeleteChargeGroupCommandHandler : IRequestHandler<DeleteChargeGroupCommand, bool>
    {
        private readonly IChargeGroupRepository _repository;
        private readonly IMapper _mapper;

        public DeleteChargeGroupCommandHandler(IChargeGroupRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteChargeGroupCommand command, CancellationToken cancellationToken)
        {
            var deleted = await _repository.DeleteAsync(command.Id).ConfigureAwait(false);
           
            if (deleted == 0)
            {
                throw new ChargeGroupNotFoundException(command.Id);
            }
            return true;
        }

      

       
    }
}
