using Blog.Application.Dtos.Authentication;
using Blog.Application.Dtos.User;
using Blog.Application.Dtos.Users;

namespace Blog.Application.Services.Users;

public interface IUserService
{
    Task<TokenDto> CreateAsync(UserCreationDto user);
    Task<TokenDto> LoginAsync(LoginDto loginData);
    Task<TokenDto> RefreshAsync(string refreshToken, string username);
    Task DeleteAsync(string username);
    Task<UserDto> EditAsync(string username, UserUpdateDto updatedUser);
    Task<UserDto> GetByUsernameAsync(string username);
    Task<UserDto> VerifyUserAsync(string username);
}