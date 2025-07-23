using Blog.Domain.Entities;

namespace Blog.Shared.Security;

public interface IPasswordHasher
{
    public string HashPassword(User user, string password);
    public bool PasswordEquals(User user, string password);
}