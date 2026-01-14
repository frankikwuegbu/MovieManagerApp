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
        private readonly JwtAuthTokenService authToken;
        private readonly EmailSenderService emailSender;

        public UserController(UserManager<User> userManager, JwtAuthTokenService authToken, EmailSenderService emailSender)
        {
            this.userManager = userManager;
            this.authToken = authToken;
            this.emailSender = emailSender;
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

            var subject = "REGISTRATION COMPLETE";
            var message = $"{regUserDto.FullName} your registration is complete.\n" +
                $"You can now get tickets to see any movie of your choice";

            await emailSender.SendEmailAsync(regUserDto.UserName, subject, message);

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
