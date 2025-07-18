using Blog.Domain.Entities;
using Blog.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Blog.Shared.Security;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(User user, string password) =>  new PasswordHasher<User>().HashPassword(user, password);
    
    public void VerifyPassword(User user, string hashedPassword, string password)
    {
        var result = new PasswordHasher<User>().VerifyHashedPassword(user, hashedPassword, password);

        if (result == PasswordVerificationResult.Failed)
            throw new IncorrectPasswordException();
    }
}