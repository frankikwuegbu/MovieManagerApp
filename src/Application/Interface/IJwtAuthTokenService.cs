using Application.Entities;

namespace Application.Interface;

public interface IJwtAuthTokenService
{
    Task<string> JwtTokenGenerator(User user);
}