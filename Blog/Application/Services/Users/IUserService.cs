using Blog.Application.Dtos.User;
using Blog.Application.Dtos.Users;

namespace Blog.Application.Services.Users;

public interface IUserService
{
    Task Create(UserCreationDto user);
    Task Delete(string username);
    Task<UserDto> Edit(string username, UserUpdateDto updatedUser);
    Task<UserDto> GetByUsername(string username);
    Task<UserDto> VerifyUser(string username);
}