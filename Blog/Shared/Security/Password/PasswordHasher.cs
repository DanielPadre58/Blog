using Blog.Domain.Entities;
using Blog.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Blog.Shared.Security;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(User user, string password) => new PasswordHasher<User>().HashPassword(user, password);
    
    public bool PasswordEquals(User user, string password)
     {
         var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, password);
 
         return result == PasswordVerificationResult.Success;
     }
 }