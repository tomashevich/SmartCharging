using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartCharge.Application.Commands.ChargeGroupCommands;

using SmartCharge.Application.Queries;

namespace Presentation.Controllers
{
    public class ChargeStationController : BaseController
    {

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<GetChargeGroupDto>>> GetChargeStation(Guid id)
        {
            var response = await Mediator.Send(new GetChargeGroupQuery{ Id = id});

            return Ok(response);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<AddChargeGroupDto>> CreateChargeStation(AddChargeGroupCommand command)
        {
            var response = await Mediator.Send(command);

            return CreatedAtAction(nameof(CreateChargeStation), response);
        }


        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<UpdateChargeGroupDto>>> UpdateChargeStation(Guid id, [FromBody] UpdateChargeGroupRequest request)
        {
            var response = await Mediator.Send(new UpdateChargeGroupCommand { Id = id, Name = request.Name, Capacity= request.CapacityAmps});

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<bool> >DeleteChargeStation(Guid id)
        {
            var response = await Mediator.Send(new DeleteChargeGroupCommand { Id = id });

            return Ok(true);
        }
    }
}