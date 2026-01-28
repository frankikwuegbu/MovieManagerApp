using Application.Features.Users.LoginUser;
using Application.Features.Users.RegisterUser;
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
        public async Task<IActionResult> Register(RegisterUserCommand request)
        {
            var user = await _sender.Send(request);

            return Ok(user);
        }

        //login user
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserCommand request)
        {
            var user = await _sender.Send(request);

            return Ok(user);
        }
    }
}