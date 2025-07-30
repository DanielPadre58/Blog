using Blog.Application.Dtos.Authentication;
using Blog.Application.Dtos.User;
using Blog.Application.Dtos.Users;
using Blog.Domain.Entities;

namespace Blog.Application.Services.Users;

public interface IUserService
{
    Task<string> CreateAsync(UserCreationDto user);
    Task<TokenDto> LoginAsync(LoginDto loginData);
    Task<TokenDto> RefreshAsync(string refreshToken, string username);
    Task DeleteAsync(string username);
    Task<UserDto> EditAsync(string username, UserUpdateDto updatedUser);
    Task<UserDto> GetByUsernameAsync(string username);
    Task<TokenDto> VerifyUserAsync(string username);
    Task<bool> LikeAsync(Post post, string username);
    Task<bool> LikeAsync(Comment comment, string username);
    Task<bool> DislikeAsync(Post post, string username);
    Task<bool> DislikeAsync(Comment comment, string username);
    Task<bool> UserLikedAsync(Post post, string username);
    Task<bool> UserLikedAsync(Comment comment, string username);
    Task<bool> UserDislikedAsync(Post post, string username);
    Task<bool> UserDislikedAsync(Comment comment, string username);
}