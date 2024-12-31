namespace Application.Dtos;

public record RegistrationDto(
    UserDto User,
    EventDto Event,
    DateTime RegistrationDate
);
