using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Common;

public class BaseMovieManagerRepository(IMapper mapper, ILogger logger)
{
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;
}
