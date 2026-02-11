using Application;
using Application.Entities;
using Application.Features.Users.Command;
using Application.Users.Command;
using Application.Users.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        //register user
        [HttpPost("register")]
        public async Task<ActionResult<Result>> Register(RegisterUserCommand request)
        {
            return await _sender.Send(request);
        }

        //login user
        [HttpPost("login")]
        public async Task<ActionResult<Result>> Login(LoginUserCommand request)
        {
            return await _sender.Send(request);
        }

        //get all users
        [HttpGet]
        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        public async Task<ActionResult<Result>> GetAllUsers([FromQuery] GetAllUsersQuery request)
        {
            return await _sender.Send(request);
        }

        //get user by id
        [HttpGet("{id}")]
        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        public async Task<ActionResult<Result>> GetUserById(string id)
        {
            return await _sender.Send(new GetUserByIdQuery(id));
        }
    }
}