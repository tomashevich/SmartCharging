using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Posts.Commands.CreatePost;
using Application.Posts.Queries.GetAllPosts;
using Microsoft.AspNetCore.Mvc;
using SmartCharge.Application.Commands.ChargeGroupCommands;
using SmartCharge.Application.Posts.Commands.ChargeGroupCommands;

namespace Presentation.Controllers
{
    public class ChargeGroupController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllPostsDto>>> GetAllPosts()
        {
            var response = await Mediator.Send(new GetAllPostsQuery());

            return Ok(response);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<AddChargeGroupDto>> CreateChargeGroup(AddChargeGroupCommand command)
        {
            var response = await Mediator.Send(command);

            return CreatedAtAction(nameof(CreateChargeGroup), response);
        }
    }
}