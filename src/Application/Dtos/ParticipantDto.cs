namespace Application.Dtos;

public record class ParticipantDto(
    UserDto User,
    DateTime RegistrationDate
);
