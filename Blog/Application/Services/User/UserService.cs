using System.Net;
using Blog.Api;
using Blog.Application.Dtos.User;
using Blog.Persistence.Repositories;

namespace Blog.Application.Services.User;

public class UserService(IUserRepo repository) : IUserService
{
    public async Task<ResponseModel<Domain.Entities.User>> Create(UserCreationDto userDto)
    {
        var response = new ResponseModel<Domain.Entities.User>();
        try
        {
            var user = new Domain.Entities.User()
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Password = userDto.Password
            };
            
            await repository.Create(user);
            
            response.Status = HttpStatusCode.Created;
            response.Message = "User created";
            response.Data = new List<Domain.Entities.User> { user };
        }
        catch (Exception ex)
        {
            response.Status = HttpStatusCode.BadRequest;
            response.Message = $"Error creating user: {ex.Message}";
        }
        
        return response;
    }
}