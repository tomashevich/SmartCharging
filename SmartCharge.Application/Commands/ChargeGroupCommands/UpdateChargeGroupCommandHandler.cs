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
        private readonly IChargeGroupRepository _chargeGrouprepository;
        private readonly IMapper _mapper;

        public UpdateChargeGroupCommandHandler(IChargeGroupRepository chargeGrouprepository, IMapper mapper)
        {
            _chargeGrouprepository = chargeGrouprepository;
            _mapper = mapper;
        }

        public async Task<UpdateChargeGroupDto> Handle(UpdateChargeGroupCommand command, CancellationToken cancellationToken)
        {
            var chargeGroup = await _chargeGrouprepository.GetAsyncExtended(command.Id).ConfigureAwait(false);
            if (chargeGroup == null)
            {
                throw new ChargeGroupNotFoundException(command.Id);
            }

            var result = chargeGroup.Update(command.Name, command.Capacity);
            if (result.IsError)
            {
                return new UpdateChargeGroupDto
                {
                    IsError = true,
                    ErrorMessage = "ChargeGroup capacity exceeded. You can unplug these connectors:",
                    ConnectorsToUnplug = result.Suggestions.ToResultStrings()
                };
            }

            await _chargeGrouprepository.UpdateAsync(chargeGroup).ConfigureAwait(false);
            return _mapper.Map<UpdateChargeGroupDto>(chargeGroup);
        }
    }
}
