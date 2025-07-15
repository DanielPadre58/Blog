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

    public async Task<ResponseModel<Domain.Entities.User>> GetById(int userId)
    {
        var response = new ResponseModel<Domain.Entities.User>();
        try
        {
            var user = await repository.GetById(userId);

            response.Status = HttpStatusCode.OK;
            response.Message = "User retrieved successfully";
            response.Data = new List<Domain.Entities.User> { user };
        }
        catch (NullReferenceException ex)
        {
            response.Status = HttpStatusCode.NotFound;
            response.Message = $"User with id {userId} does not exist";
        }
        catch (Exception ex)
        {
            response.Status = HttpStatusCode.BadGateway;
            response.Message = $"Error retrieving user: {ex.Message}";
        }

        return response;
    }
}