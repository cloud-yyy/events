namespace Domain.Authentication;

public interface ICurrentUserAccessor
{
    public Guid? UserId { get; }
    public string? Role { get; }
}
