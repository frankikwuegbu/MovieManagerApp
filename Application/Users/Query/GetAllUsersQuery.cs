using Application.Features.Users;
using Application.Interface;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Query;


public record GetAllUsersQuery() : IRequest<Result>;
public class GetAllUsersQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetAllUsersQuery, Result>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .AsNoTracking().ToListAsync(cancellationToken);

        if(users is null)
        {
            return Result.Failure("empty users list!");
        }

        var usersDto = _mapper.Map<List<UserDto>>(users);

        return Result.Success("success!", usersDto);
    }
}
