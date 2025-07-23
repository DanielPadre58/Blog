using Blog.Application.Dtos.User;
using Blog.Application.Dtos.Users;

namespace Blog.Application.Services.Users;

public interface IUserService
{
    Task CreateAsync(UserCreationDto user);
    Task DeleteAsync(string username);
    Task<UserDto> EditAsync(string username, UserUpdateDto updatedUser);
    Task<UserDto> GetByUsernameAsync(string username);
    Task<UserDto> VerifyUserAsync(string username);
}