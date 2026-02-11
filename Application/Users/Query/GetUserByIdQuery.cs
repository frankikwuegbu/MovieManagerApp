using Application.Features.Users;
using Application.Interface;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Query;

public record GetUserByIdQuery(string Id) : IRequest<Result>;
public class GetUserByIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetUserByIdQuery, Result>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id,
            cancellationToken);

        if (user == null)
        {
            return Result.Failure("user not found");
        }

        var userDto = _mapper.Map<UserByIdDto>(user);

        return Result.Success("user found!", userDto);
    }
}
