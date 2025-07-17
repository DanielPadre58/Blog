using Blog.Application.Dtos.User;
using Blog.Application.Dtos.Users;

namespace Blog.Application.Services.Users;

public interface IUserService
{
    Task<UserDto> Create(UserCreationDto user);
    Task DeleteById(int userId);
    Task<UserDto> EditById(int id, UserUpdateDto updatedUser);
    Task<UserDto> GetById(int id);
    Task<List<UserDto>> GetByUsername(string username);
}