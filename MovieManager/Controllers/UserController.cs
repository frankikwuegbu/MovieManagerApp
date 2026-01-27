using Application.Features.Users.LoginUser;
using Application.Features.Users.RegisterUser;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieManager.Models.Dtos;

namespace MovieManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public UserController(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        //register user
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            var request = _mapper.Map<RegisterUserCommand>(registerUserDto);
            var user = await _sender.Send(request);

            return Ok(user);
        }

        //login user
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            var request = _mapper.Map<LoginUserCommand>(loginUserDto);
            var user = await _sender.Send(request);

            return Ok(user);
        }
    }
}