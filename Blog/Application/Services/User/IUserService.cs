using Blog.Api;
using Blog.Application.Dtos.User;

namespace Blog.Application.Services.User;

public interface IUserService
{
    public Task<ResponseModel<Domain.Entities.User>>? Create(UserCreationDto user);
}