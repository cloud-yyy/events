using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Event, EventDto>();
        CreateMap<User, UserDto>();
        CreateMap<Role, RoleDto>();
        CreateMap<Registration, ParticipantDto>();

        CreateMap<Image, ImageDto>().ForMember(
            i => i.PublicUrl, 
            opts => opts.MapFrom(i => i.ObjectKey)
        );
    }
}
