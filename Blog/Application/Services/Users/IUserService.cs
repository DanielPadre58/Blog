using Blog.Api;
using Blog.Application.Dtos.User;

namespace Blog.Application.Services.Users;

public interface IUserService
{
    public Task<ResponseModel<Domain.Entities.User>> Create(UserCreationDto user);
    public Task<ResponseModel<Domain.Entities.User>> DeleteById(int userId);
    public Task<ResponseModel<Domain.Entities.User>> EditById(int id, UserUpdateDto updatedUser);
    public Task<ResponseModel<Domain.Entities.User>>? GetById(int id);
    public Task<ResponseModel<Domain.Entities.User>> GetByUsername(string username);
}