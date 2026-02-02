using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Common;

public class BaseMovieManagerRepository(IMapper mapper, ILogger logger) : ControllerBase
{
    private IMapper _mapper = mapper;
    private ILogger _logger = logger;
}
