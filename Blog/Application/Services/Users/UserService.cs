using System.Net;
using Blog.Api;
using Blog.Application.Dtos.User;
using Blog.Application.Dtos.Users;
using Blog.Domain.Entities;
using Blog.Persistence.Repositories.Users;

namespace Blog.Application.Services.Users;

public class UserService(IUserRepo repository) : IUserService
{
    public async Task<ResponseModel<UserDto>> Create(UserCreationDto userDto)
    {
        var response = new ResponseModel<UserDto>();
        try
        {
            if (UsernameExists(userDto.Username, response, out var failedResponse)) return failedResponse;
            
            var user = new User()
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Password = userDto.Password
            };
            
            user.Validate();

            user = repository.Create(user).Result;

            response.Status = HttpStatusCode.Created;
            response.Message = "User created";
            response.Data.Add(new UserDto(user));
        }
        catch (Exception ex)
        {
            response.Status = HttpStatusCode.BadRequest;
            response.Message = $"Error creating user: {ex.Message}";
        }

        return response;
    }

    public async Task<ResponseModel<UserDto>> DeleteById(int userId)
    {
        var response = new ResponseModel<UserDto>();
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

    public async Task<ResponseModel<UserDto>> EditById(int id, UserUpdateDto updatedUser)
    {
        var response = new ResponseModel<UserDto>();
        try
        {
            if (updatedUser.Username != null && UsernameExists(updatedUser.Username, response, out var failedResponse)) 
                return failedResponse;

            var user = await repository.EditById(id, updatedUser);
            
            response.Status = HttpStatusCode.OK;
            response.Message = "User updated successfully";
            response.Data.Add(new UserDto(user));
        }
        catch (NullReferenceException ex)
        {
            response.Status = HttpStatusCode.NotFound;
            response.Message = $"User with id {id} does not exist: {ex.Message}";
        }
        catch (Exception ex)
        {
            response.Status = HttpStatusCode.BadGateway;
            response.Message = $"Error updating user: {ex.Message}";
        }

        return response;
    }

    public async Task<ResponseModel<UserDto>> GetById(int userId)
    {
        var response = new ResponseModel<UserDto>();
        try
        {
            var user = await repository.GetById(userId);

            response.Status = HttpStatusCode.OK;
            response.Message = "User retrieved successfully";
            response.Data.Add(new UserDto(user));
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
    
    public async Task<ResponseModel<UserDto>> GetByUsername(string username)
    {
        var response = new ResponseModel<UserDto>();
        try
        {
            var users = await repository.GetByUsernameUncapitalized(username.ToLower());

            response.Status = HttpStatusCode.OK;
            response.Message = "User(s) retrieved successfully";
            response.Data = UserDto.ToDtoList(users);
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

    public async Task<ResponseModel<UserDto>> AddLikeById(int userId, int postId)
    {
        var response = new ResponseModel<UserDto>();
        try
        {
            await repository.AddLikeById(userId, postId);

            response.Status = HttpStatusCode.OK;
            response.Message = "Like added successfully";
        }
        catch (NullReferenceException ex)
        {
            response.Status = HttpStatusCode.NotFound;
            response.Message = $"User or post not found: {ex.Message}";
        }
        catch (Exception ex)
        {
            response.Status = HttpStatusCode.BadGateway;
            response.Message = $"Error adding like: {ex.Message}";
        }

        return response;
    }

    private bool UsernameExists(string username, ResponseModel<UserDto> response, out ResponseModel<UserDto> failedResponse)
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