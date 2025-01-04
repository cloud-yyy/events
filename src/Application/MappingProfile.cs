using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Event, EventDto>();
        CreateMap<User, UserDto>();
        CreateMap<Role, RoleDto>();
        CreateMap<Registration, ParticipantDto>();
        CreateMap<Category, CategoryDto>();

        CreateMap<Image, ImageDto>()
        .ForMember(
            dest => dest.PublicUrl,
            opt => opt.MapFrom<ImageUrlValueResolver>()
        );
    }
}
