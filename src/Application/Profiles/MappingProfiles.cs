using Application.Features.Movies.Command;
using AutoMapper;
using Application.Entities;
using Application.Features.Movies;
using Application.Features.Users;
using Application.Users.Command;
using Domain.Entities;

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
            CreateMap<User, UserByIdDto>();
            CreateMap<Movie, MovieByIdDto>();
            CreateMap<Movie, MovieDto>();
        }
    }
}