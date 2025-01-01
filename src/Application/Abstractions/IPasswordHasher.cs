namespace Application.Abstractions;

public interface IPasswordHasher
{
    public string HashPassword(string password);
}
