using Blog.Application.Dtos.User;
using Blog.Application.Dtos.Users;
using Blog.Application.External_Services;
using Blog.Domain.Entities;
using Blog.Domain.Repositories.Users;
using Blog.Shared.Exceptions;
using Blog.Shared.Security;

namespace Blog.Application.Services.Users;

public class UserService(IUserRepo repository, IPasswordHasher hasher, ISmtpService smpt, IUnvalidatedUsersRepo unvalidatedUsersRepo) : IUserService
{
    public async Task Create(UserCreationDto userDto)
    {
        await ValidateUsernameUniquenessAsync(userDto.Username);
        
        var user = new User
        {
            Username = userDto.Username,
            Email = userDto.Email,
            Password = string.Empty
        };

        user.Password = hasher.HashPassword(user, userDto.Password);
        
        user.Validate();

        var createdUser = await repository.Create(user);

        await SendVerificationEmailAsync(createdUser);
    }

    public async Task Delete(string username)
    {
        await repository.Delete(username);
    }

    public async Task<UserDto> Edit(string username, UserUpdateDto updatedUser)
    {
        updatedUser.Validate();

        if (updatedUser.Username != null)
            await ValidateUsernameUniquenessAsync(updatedUser.Username);

        var user = await repository.Edit(username, updatedUser);
        return new UserDto(user);
    }

    public async Task<UserDto> GetByUsername(string username)
    {
        var user = await repository.GetByUsername(username.ToLower());
        
        if(!user.verified)
            throw new UnverifiedUserException();
        
        return new UserDto(user);
    }

    public async Task<UserDto> VerifyUser(string validationCode)
    {
        var username = unvalidatedUsersRepo.ValidateUser(validationCode).Result;

        var user = await repository.VerifyUser(username);

        return new UserDto(user);
    }
    
    private async Task SendVerificationEmailAsync(User user)
    {
        var validationCode = Guid.NewGuid().ToString();
        await unvalidatedUsersRepo.AddValidationCode(validationCode, user.Username);

        await smpt.SendVerificationCodeAsync(user.Email, validationCode);
    }

    private async Task ValidateUsernameUniquenessAsync(string username)
    {
        if (await repository.UsernameExists(username))
            throw new DuplicatedUsernameException(username);
    }
}