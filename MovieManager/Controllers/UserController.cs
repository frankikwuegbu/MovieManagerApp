using Application.Features.Users.Command;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MovieManager.Controllers
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
    }
}