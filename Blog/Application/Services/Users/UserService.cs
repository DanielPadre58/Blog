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

            response.Created(
                "User created successfully",
                new UserDto(user)
            );
        }
        catch (ArgumentException ex)
        {
            response.BadRequest($"Invalid user data: {ex.Message}");
        }
        catch (Exception ex)
        {
            response.InternalServerError($"Error creating user: {ex.Message}");
        }

        return response;
    }

    public async Task<ResponseModel<UserDto>> DeleteById(int userId)
    {
        var response = new ResponseModel<UserDto>();

        try
        {
            await repository.Delete(userId);

            response.Ok("User deleted successfully");
        }
        catch (NullReferenceException ex)
        {
            response.NotFound($"User with id {userId} not found: {ex.Message}");
        }
        catch (Exception ex)
        {
            response.InternalServerError($"Error deleting user: {ex.Message}");
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

            updatedUser.Validate();

            var user = await repository.EditById(id, updatedUser);

            response.Ok(
                "User updated successfully",
                new UserDto(user)
            );
        }
        catch (NullReferenceException ex)
        {
            response.NotFound($"User with id {id} does not exist: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            response.BadRequest($"Invalid user data: {ex.Message}");
        }
        catch (Exception ex)
        {
            response.BadRequest($"Error editing user: {ex.Message}");
        }

        return response;
    }

    public async Task<ResponseModel<UserDto>> GetById(int userId)
    {
        var response = new ResponseModel<UserDto>();

        try
        {
            var user = await repository.GetById(userId);

            response.Ok(
                "User retrieved successfully",
                new UserDto(user)
            );
        }
        catch (NullReferenceException ex)
        {
            response.NotFound($"User with id {userId} does not exist: {ex.Message}");
        }
        catch (Exception ex)
        {
            response.InternalServerError($"Error getting user: {ex.Message}");
        }

        return response;
    }

    public async Task<ResponseModel<UserDto>> GetByUsername(string username)
    {
        var response = new ResponseModel<UserDto>();
        try
        {
            var users = await repository.GetByUsernameUncapitalized(username.ToLower());

            response.Ok(
                "User(s) retrieved successfully",
                UserDto.ToDtoList(users)
            );
        }
        catch (NullReferenceException ex)
        {
            response.NotFound($"User with username {username} does not exist: {ex.Message}");
        }
        catch (Exception ex)
        {
            response.InternalServerError($"Error getting user: {ex.Message}");
        }

        return response;
    }

    public async Task<ResponseModel<UserDto>> AddLikeById(int userId, int postId)
    {
        var response = new ResponseModel<UserDto>();
        
        try
        {
            await repository.AddLikeById(userId, postId);

            response.Ok("Like added successfully");
        }
        catch (NullReferenceException ex)
        {
            response.NotFound($"User or post not found: {ex.Message}");
        }
        catch (Exception ex)
        {
            response.InternalServerError($"Error adding like: {ex.Message}");
        }

        return response;
    }

    private bool UsernameExists(string username, ResponseModel<UserDto> response,
        out ResponseModel<UserDto> failedResponse)
    {
        if (repository.UsernameExists(username).Result)
        {
            response.Conflict("User with same username already exists");
            failedResponse = response;
            return true;
        }

        failedResponse = response;
        return false;
    }
}