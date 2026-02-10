using Application.Features.Movies.Command;
using Application.Features.Users.Command;
using AutoMapper;
using Application.Entities;
using Application.Features.Movies;
using Application.Features.Users;

namespace Application.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AddMovieCommand, Movie>();
            CreateMap<UpdateMovieCommand, Movie>();
            CreateMap<RegisterUserCommand, User>();
            CreateMap<User, UserDto>();
            CreateMap<Movie, MovieByIdDto>();
        }
    }
}