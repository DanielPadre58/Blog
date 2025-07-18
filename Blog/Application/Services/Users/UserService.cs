using Blog.Application.Dtos.User;
using Blog.Application.Dtos.Users;
using Blog.Domain.Entities;
using Blog.Domain.Repositories.Users;
using Blog.Shared.Exceptions;

namespace Blog.Application.Services.Users;

public class UserService(IUserRepo repository) : IUserService
{
    public async Task<UserDto> Create(UserCreationDto userDto)
    {
        await ValidateUsernameUniquenessAsync(userDto.Username);

        var user = new User
        {
            Username = userDto.Username,
            Email = userDto.Email,
            Password = userDto.Password
        };

        user.Validate();

        var createdUser = await repository.Create(user);

        return new UserDto(createdUser);
    }

    public async Task DeleteById(int userId)
    {
        await repository.Delete(userId);
    }

    public async Task<UserDto> EditById(int id, UserUpdateDto updatedUser)
    {
        updatedUser.Validate();

        if (updatedUser.Username != null)
            await ValidateUsernameUniquenessAsync(updatedUser.Username);

        var user = await repository.EditById(id, updatedUser);
        return new UserDto(user);
    }

    public async Task<UserDto> GetById(int userId)
    {
        var user = await repository.GetById(userId);
        return new UserDto(user);
    }

    public async Task<List<UserDto>> GetByUsername(string username)
    {
        var users = await repository.GetByUsernameUncapitalized(username.ToLower());
        return users.Select(u => new UserDto(u)).ToList();
    }

    private async Task ValidateUsernameUniquenessAsync(string username)
    {
        if (await repository.UsernameExists(username))
            throw new DuplicatedUsernameException(username);
    }
}