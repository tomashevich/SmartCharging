using AutoMapper;
using MediatR;
using SmartCharge.Application.Exceptions;
using SmartCharge.Core.Entities;
using SmartCharge.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{

    internal sealed class AddConnectorCommandHandler : IRequestHandler<AddConnectorCommand, UpdateChargeStationDto>
    {
        private readonly IChargeGroupRepository _groupRepository;

        private readonly IChargeStationRepository _stationRepository;
        private readonly IMapper _mapper;

        public AddConnectorCommandHandler(IChargeGroupRepository groupRepository, IChargeStationRepository stationRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _stationRepository = stationRepository;
            _mapper = mapper;
        }

        public async Task<UpdateChargeStationDto> Handle(AddConnectorCommand command, CancellationToken cancellationToken)
        {
            var found = await _stationRepository.ExistsAsync(command.ChargeStationId).ConfigureAwait(false);

            if (!found)
            {
                throw new ChargeStationNotFoundException(command.ChargeStationId);
            }


            var chargeStation = await _stationRepository.GetAsyncExtended(command.ChargeStationId).ConfigureAwait(false);

            
            //add connector to station
            chargeStation.AddConnector(command.ConnectorMaxCurrentAmps, command.ConnectorId);

    

            await _stationRepository.UpdateConnectorsAsync(chargeStation).ConfigureAwait(false);

            
           
            

            return _mapper.Map<UpdateChargeStationDto>(chargeStation);
        }
              
    }
}
