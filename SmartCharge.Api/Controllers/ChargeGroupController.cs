using Microsoft.AspNetCore.Mvc;
using SmartCharge.Application.Commands.ChargeGroupCommands;
using SmartCharge.Application.Queries;
using System;
using System.Threading.Tasks;

namespace SmartCharge.Api.Controllers
{
    public class ChargeGroupController : BaseController
    {
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GetChargeGroupDto>> GetChargeGroup(Guid id)
        {
            var response = await Mediator.Send(new GetChargeGroupQuery { Id = id });

            return Ok(response);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<AddChargeGroupDto>> CreateChargeGroup(AddChargeGroupCommand command)
        {
            var response = await Mediator.Send(command);

            return CreatedAtAction(nameof(CreateChargeGroup), response);
        }


        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult<UpdateChargeGroupDto>> UpdateChargeGroup(Guid id, 
            [FromBody] UpdateChargeGroupRequest request)
        {
            var response = await Mediator.Send(new UpdateChargeGroupCommand
            {
                Id = id,
                Name = request.Name,
                Capacity = request.CapacityAmps
            });

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<bool>> DeleteChargeGroup(Guid id)
        {
            var response = await Mediator.Send(new DeleteChargeGroupCommand { Id = id });

            return Ok(true);
        }
    }
}