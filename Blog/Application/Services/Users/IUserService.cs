using Blog.Api;
using Blog.Application.Dtos.User;
using Blog.Application.Dtos.Users;

namespace Blog.Application.Services.Users;

public interface IUserService
{
    public Task<ResponseModel<UserDto>> Create(UserCreationDto user);
    public Task<ResponseModel<UserDto>> DeleteById(int userId);
    public Task<ResponseModel<UserDto>> EditById(int id, UserUpdateDto updatedUser);
    public Task<ResponseModel<UserDto>>? GetById(int id);
    public Task<ResponseModel<UserDto>> GetByUsername(string username);
    public Task<ResponseModel<UserDto>> AddLikeById(int userId, int postId);
}