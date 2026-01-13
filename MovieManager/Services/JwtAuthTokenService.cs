using Azure.Core;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MovieManager.Models.Entities;
using System.Collections;
using System.Security.Claims;
using System.Text;

namespace MovieManager.Services;

public class JwtAuthTokenService
{
    private readonly IConfiguration config;
    private readonly UserManager<User> userManager;

    public JwtAuthTokenService(IConfiguration config, UserManager<User> userManager)
    {
        this.config = config;
        this.userManager = userManager;
    }
    public async Task<string> JwtTokenGenerator(User user)
    {
        var roles = await userManager.GetRolesAsync(user);

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["JWT:SecretKey"]!));

        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claim =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Name, user.UserName!),
            ..roles.Select(r => new Claim(ClaimTypes.Role, r))  //potential error here
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
