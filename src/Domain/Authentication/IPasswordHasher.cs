namespace Domain.Authentication;

public interface IPasswordHasher
{
    public string HashPassword(string password);
}
