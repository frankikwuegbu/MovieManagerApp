using Application.Features.Movies.UpdateMovie;
using Application.Features.Users.LoginUser;
using Application.Features.Users.RegisterUser;
using AutoMapper;
using MovieManager.Controllers.Commands.AddMovie;
using MovieManager.Models.Dtos;
using MovieManager.Models.Entities;

namespace Application.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AddMovieDto, AddMovieCommand>();
            CreateMap<UpdateMovieDto, UpdateMovieCommand>();
            CreateMap<RegisterUserDto, RegisterUserCommand>();
            CreateMap<LoginUserDto, LoginUserCommand>();
            CreateMap<AddMovieCommand, Movie>();
            CreateMap<UpdateMovieCommand, Movie>();
            CreateMap<RegisterUserCommand, User>();
        }
    }
}