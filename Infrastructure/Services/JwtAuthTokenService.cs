using Application.Entities;
using Application.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services;

public class JwtAuthTokenService(IConfiguration config, UserManager<User> userManager) : IJwtAuthTokenService
{
    private readonly IConfiguration config = config;
    private readonly UserManager<User> userManager = userManager;

    public async Task<string> JwtTokenGenerator(User user)
    {
        var roles = await userManager.GetRolesAsync(user);

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["JWT:SecretKey"]!));

        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claim =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Name, user.Email!),
            ..roles.Select(r => new Claim(ClaimTypes.Role, r))
        ];

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claim),
            Expires = DateTime.UtcNow.AddMinutes(config.GetValue<int>("JWT:ExpirationInMinutes")),
            SigningCredentials = credentials,
            Issuer = config["JWT:Issuer"],
            Audience = config["JWT:Audience"]
        };

        var tokenHandler = new JsonWebTokenHandler();

        string accessToken = tokenHandler.CreateToken(tokenDescriptor);

        return accessToken;
    }
}