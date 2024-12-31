using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Event, EventDto>();
        CreateMap<Image, ImageDto>();
        CreateMap<User, UserDto>();
        CreateMap<Role, RoleDto>();
        CreateMap<Registration, RegistrationDto>();
    }
}
