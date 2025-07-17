using Blog.Application.Dtos.User;
using Blog.Application.Dtos.Users;
using Blog.Application.Services.Users;
using Blog.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ResponseModel<UserDto>>> CreateUser(UserCreationDto user)
    {
        var response = new ResponseModel<UserDto>();

        try
        {
            var createdUser = await service.Create(user);
            response.SuccessResponse("User created successfully", createdUser);

            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, response);
        }
        catch (DuplicatedUsernameException ex)
        {
            return Conflict(response.ErrorResponse(ex.Message));
        }
        catch (InvalidFieldsException ex)
        {
            return BadRequest(response.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, response.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete]
    public async Task<ActionResult<ResponseModel<UserDto>>> DeleteUserById([FromQuery] int id)
    {
        var response = new ResponseModel<UserDto>();

        try
        {
            await service.DeleteById(id);
            response.SuccessResponse("User deleted successfully");
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(response.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, response.ErrorResponse(ex.Message));
        }
    }

    [HttpPut]
    public async Task<ActionResult<ResponseModel<UserDto>>> EditUserById([FromQuery] int id, [FromBody] UserUpdateDto updatedUser)
    {
        var response = new ResponseModel<UserDto>();

        try
        {
            var user = await service.EditById(id, updatedUser);
            response.SuccessResponse("User edited successfully", user);
            return Ok(response);
        }
        catch (InvalidFieldsException ex)
        {
            return BadRequest(response.ErrorResponse(ex.Message));
        }
        catch (DuplicatedUsernameException ex)
        {
            return Conflict(response.ErrorResponse(ex.Message));
        }
        catch (NotFoundException ex)
        {
            return NotFound(response.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, response.ErrorResponse(ex.Message));
        }
    }

    [HttpGet]
    public async Task<ActionResult<ResponseModel<UserDto>>> GetUser([FromQuery] int? id, [FromQuery] string? username = null)
    {
        var response = new ResponseModel<UserDto>();

        if (username != null && id.HasValue)
        {
            return BadRequest(response.ErrorResponse("Please provide either id or username, not both."));
        }

        if (username == null && !id.HasValue)
        {
            return BadRequest(response.ErrorResponse("Please provide either id or username."));
        }

        if (username == null)
        {
            return await GetUserById(id!.Value, response);
        }

        return await GetUserByUsername(username, response);
    }

    private async Task<ActionResult<ResponseModel<UserDto>>> GetUserByUsername(string username, ResponseModel<UserDto> response)
    {
        try
        {
            var users = await service.GetByUsername(username);
            response.SuccessResponse("User retrieved successfully", users);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(response.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, response.ErrorResponse(ex.Message));
        }
    }

    private async Task<ActionResult<ResponseModel<UserDto>>> GetUserById(int id, ResponseModel<UserDto> response)
    {
        try
        {
            var user = await service.GetById(id);
            response.SuccessResponse("User retrieved successfully", user);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(response.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, response.ErrorResponse(ex.Message));
        }
    }
}