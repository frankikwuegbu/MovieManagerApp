using Application.Features.Users;
using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Query;


public record GetAllUsersQuery() : IRequest<Result>;
public class GetAllUsersQueryHandler(IApplicationDbContext context) : IRequestHandler<GetAllUsersQuery, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .AsNoTracking()
            .Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName
            })
            .ToListAsync(cancellationToken);

        if(users.Count == 0)
        {
            return Result.Failure("empty users list!");
        }

        return Result.Success("success!", users);
    }
}
