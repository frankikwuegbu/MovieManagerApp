using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieManager.Models.Dtos;
using MovieManager.Models.Entities;
using MovieManager.Services;
using System.Data;

namespace MovieManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly JwtAuthTokenService authToken;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, JwtAuthTokenService authToken)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.authToken = authToken;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto regUserDto)
        {
            var user = new User
            {
                FullName = regUserDto.FullName,
                UserName = regUserDto.UserName,
                PasswordHash = regUserDto.Password,
                Role = regUserDto.Roles
            };
            var result = await userManager.CreateAsync(user, regUserDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            await userManager.AddToRoleAsync(user, regUserDto.Roles.ToString());

            return Ok("User created");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto logUserDto)
        {
            var user = await userManager.FindByNameAsync(logUserDto.UserName);

            if (user == null ||
                ! await userManager.CheckPasswordAsync(user, logUserDto.Password))
                return Unauthorized("Invalid credentials");

            var token = await authToken.JwtTokenGenerator(user);
            return Ok(token);
        }
    }
}
