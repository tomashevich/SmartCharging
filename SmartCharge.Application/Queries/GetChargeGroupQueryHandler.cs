using AutoMapper;
using MediatR;
using SmartCharge.Application.Exceptions;

using SmartCharge.Core.Entities;
using SmartCharge.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCharge.Application.Queries
{

    internal sealed class GetChargeGroupQueryHandler : IRequestHandler<GetChargeGroupQuery, GetChargeGroupDto>
    {
        private readonly IChargeGroupRepository _repository;
        private readonly IMapper _mapper;

        public GetChargeGroupQueryHandler(IChargeGroupRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetChargeGroupDto> Handle(GetChargeGroupQuery command, CancellationToken cancellationToken)
        {
            var resource = await _repository.GetAsync(command.Id).ConfigureAwait(false);
           
            if (resource == null)
            {
                throw new ChargeGroupNotFoundException(command.Id);
            }
            return _mapper.Map<GetChargeGroupDto>(resource);
        }

      

       
    }
}
