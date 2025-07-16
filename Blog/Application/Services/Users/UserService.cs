using System.Net;
using Blog.Api;
using Blog.Application.Dtos.User;
using Blog.Domain.Entities;
using Blog.Persistence.Repositories.Users;

namespace Blog.Application.Services.Users;

public class UserService(IUserRepo repository) : IUserService
{
    public async Task<ResponseModel<Domain.Entities.User>> Create(UserCreationDto userDto)
    {
        var response = new ResponseModel<Domain.Entities.User>();
        try
        {
            if (UsernameExists(userDto.Username, response, out var failedResponse)) return failedResponse;
            
            var user = new Domain.Entities.User()
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Password = userDto.Password
            };
            
            user.Validate();

            await repository.Create(user);

            response.Status = HttpStatusCode.Created;
            response.Message = "User created";
            response.Data.Add(user);
        }
        catch (Exception ex)
        {
            response.Status = HttpStatusCode.BadRequest;
            response.Message = $"Error creating user: {ex.Message}";
        }

        return response;
    }

    public async Task<ResponseModel<Domain.Entities.User>> DeleteById(int userId)
    {
        var response = new ResponseModel<Domain.Entities.User>();
        try
        {
            await repository.Delete(userId);

            response.Status = HttpStatusCode.OK;
            response.Message = "User deleted";
        }
        catch (Exception ex)
        {
            response.Status = HttpStatusCode.BadRequest;
            response.Message = $"Error deleting user: {ex.Message}";
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
            response.Data.Add(user);
        }
        catch (NullReferenceException ex)
        {
            response.Status = HttpStatusCode.NotFound;
            response.Message = $"User with id {userId} does not exist: {ex.Message}";
        }
        catch (Exception ex)
        {
            response.Status = HttpStatusCode.BadGateway;
            response.Message = $"Error retrieving user: {ex.Message}";
        }

        return response;
    }
    
    public async Task<ResponseModel<Domain.Entities.User>> GetByUsername(string username)
    {
        var response = new ResponseModel<Domain.Entities.User>();
        try
        {
            var users = await repository.GetByUsernameUncapitalized(username.ToLower());

            response.Status = HttpStatusCode.OK;
            response.Message = "User(s) retrieved successfully";
            response.Data = users;
        }
        catch (NullReferenceException ex)
        {
            response.Status = HttpStatusCode.NotFound;
            response.Message = $"No user found with username {username}, or  similar: {ex.Message}";
        }
        catch (Exception ex)
        {
            response.Status = HttpStatusCode.BadGateway;
            response.Message = $"Error retrieving users: {ex.Message}";
        }

        return response;
    }
    
    private bool UsernameExists(string username, ResponseModel<Domain.Entities.User> response, out ResponseModel<Domain.Entities.User> failedResponse)
    {
        if(repository.UsernameExists(username).Result)
        {
            response.Status = HttpStatusCode.Conflict;
            response.Message = "User with same username already exists";
            failedResponse = response;
            return true;
        }

        failedResponse = response;
        return false;
    }
}