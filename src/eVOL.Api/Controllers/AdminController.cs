using eVOL.Application.Features.AdminCases.Commands.AdminDeleteUser;
using eVOL.Application.Features.AdminCases.Queries.AdminGetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVOL.API.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {

        private readonly ISender _sender;
        public AdminController(ISender sender) => _sender = sender;


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserInfo(Guid id)
        {
            var user = await _sender.Send(new AdminGetUserQuery(id));

            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id) 
        {
            var user = await _sender.Send(new AdminDeleteUserCommand(id));

            if (user == null) return NotFound();

            return Ok(user);
        }


    }
}
