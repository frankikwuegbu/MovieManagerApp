using Application.Features.Movies.AddMovie;
using Application.Features.Movies.UpdateMovie;
using Application.Features.Users.RegisterUser;
using AutoMapper;
using MovieManager.Models.Entities;

namespace Application.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AddMovieCommand, Movie>();
            CreateMap<UpdateMovieCommand, Movie>();
            CreateMap<RegisterUserCommand, User>();
        }
    }
}