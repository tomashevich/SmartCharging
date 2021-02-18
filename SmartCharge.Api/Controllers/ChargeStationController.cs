using Microsoft.AspNetCore.Mvc;
using SmartCharge.Application.Commands.ChargeGroupCommands;
using SmartCharge.Application.Commands.ChargeStationCommands;
using System;
using System.Threading.Tasks;

namespace SmartCharge.Api.Controllers
{
    public class ChargeStationController : BaseController
    {
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GetChargeStationDto>> GetChargeStation(Guid id)
        {
            var response = await Mediator.Send(new GetChargeStationQuery { Id = id });

            return Ok(response);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<ChargeStationDto>> CreateChargeStation(AddChargeStationCommand command)
        {
            var response = await Mediator.Send(command);

            return CreatedAtAction(nameof(CreateChargeStation), response);
        }

        [HttpPost]
        [Route("{id}/change-name")]
        public async Task<ActionResult<UpdateChargeStationDto>> UpdateChargeStationName(Guid id,
            [FromBody] UpdateChargeStationNameRequest request)
        {
            var response = await Mediator.Send(
                new UpdateChargeStationNameCommand
                {
                    Id = id,
                    Name = request.Name
                });

            return Ok(response);
        }

        [HttpPost]
        [Route("{id}/add-connector")]
        public async Task<ActionResult<UpdateChargeStationDto>> AddConnector(Guid id,
            [FromBody] ConnectorRequest request)
        {
            var response = await Mediator.Send(
                new AddConnectorCommand
                {
                    ChargeStationId = id,
                    ConnectorId = request.ConnectorId,
                    ConnectorMaxCurrentAmps = request.MaxCurrentAmps
                });

            return Ok(response);
        }

        [HttpPost]
        [Route("{id}/change-connector")]
        public async Task<ActionResult<UpdateChargeStationDto>> ChangeConnector(Guid id,
            [FromBody] ConnectorRequest request)
        {
            var response = await Mediator.Send(
                new UpdateConnectorCommand
                {
                    ChargeStationId = id,
                    ConnectorId = request.ConnectorId,
                    ConnectorMaxCurrentAmps = request.MaxCurrentAmps
                });

            return Ok(response);
        }

        [HttpPost]
        [Route("{id}/change-group")]
        public async Task<ActionResult<UpdateChargeStationDto>> ChangeGroup(Guid id,
            [FromBody] ChangeGroupRequest request)
        {
            var response = await Mediator.Send(
                new ChangeGroupCommand
                {
                    ChargeStationId = id,
                    ChargeGroupId = request.ChargeGroupId
                });

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<bool>> DeleteChargeStation(Guid id)
        {
            var response = await Mediator.Send(new DeleteChargeStationCommand { Id = id });

            return Ok(true);
        }
    }
}