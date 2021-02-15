using SmartCharge.Application.Exceptions;
using SmartCharge.Core.Entities;
using SmartCharge.Core.Repositories;
using System.Threading.Tasks;

namespace SmartCharge.Application.Commands.Handlers
{

    internal sealed class AddChargeGroupHandler 
    {
        private readonly IChargeGroupRepository _repository;
     

        public AddChargeGroupHandler(IChargeGroupRepository repository)
        {
            _repository = repository;
           
        }

        public async Task HandleAsync(AddChargeGroup command)
        {
            if (await _repository.ExistsAsync(command.Id))
            {
                throw new ChargeGroupAlreadyExistException(command.Id);
            }

            var resource = ChargeGroup.Create(command.Id, command.Name, command.Capacity);
            await _repository.AddAsync(resource);
          
        }

       
    }
}
