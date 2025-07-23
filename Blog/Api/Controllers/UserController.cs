using Blog.Application.Dtos.Authentication;
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
    public async Task<ActionResult<ResponseModel<TokenDto>>> CreateUser(UserCreationDto user)
    {
        var response = new ResponseModel<TokenDto>();

        try
        {
            var token = await service.CreateAsync(user);
            response.SuccessResponse("User created successfully", new TokenDto(user.Username, token));
            return CreatedAtAction(nameof(GetUser), new { username = user.Username }, response);
        }
        catch (DuplicatedUsernameException ex)
        {
            return Conflict(response.ErrorResponse(ex.Message));
        }
        catch (DuplicatedEmailException ex)
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
    
    [HttpPost("login")]
    public async Task<ActionResult<ResponseModel<TokenDto>>> LoginUser([FromBody] LoginDto loginData)
    {
        var response = new ResponseModel<TokenDto>();

        try
        {
            var token = await service.LoginAsync(loginData);
            response.SuccessResponse("Login successful", new TokenDto(loginData.Username, token));
            return Ok(response);
        }
        catch (InvalidAuthenticationData ex)
        {
            return BadRequest(response.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, response.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{username}")]
    public async Task<ActionResult<ResponseModel<UserDto>>> DeleteUser(string username)
    {
        var response = new ResponseModel<UserDto>();

        try
        {
            await service.DeleteAsync(username);
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

    [HttpPut("{username}")]
    public async Task<ActionResult<ResponseModel<UserDto>>> EditUser(string username, [FromBody] UserUpdateDto updatedUser)
    {
        var response = new ResponseModel<UserDto>();

        try
        {
            var user = await service.EditAsync(username, updatedUser);
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

    [HttpGet("{username}")]
    public async Task<ActionResult<ResponseModel<UserDto>>> GetUser(string username)
    {
        var response = new ResponseModel<UserDto>();

        try
        {
            var users = await service.GetByUsernameAsync(username);
            response.SuccessResponse("User retrieved successfully", users);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(response.ErrorResponse(ex.Message));
        }
        catch (UnverifiedUserException ex)
        {
            return BadRequest(response.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, response.ErrorResponse(ex.Message));
        }
    }

    [HttpPost("verify/{validationCode}")]
    public async Task<ActionResult<ResponseModel<UserDto>>> VerifyUser(string validationCode)
    {
        var response = new ResponseModel<UserDto>();

        try
        {
            var user = await service.VerifyUserAsync(validationCode);
            response.SuccessResponse("User verified successfully", user);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(response.ErrorResponse(ex.Message));
        }
        catch (UnverifiedUserException ex)
        {
            return BadRequest(response.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, response.ErrorResponse(ex.Message));
        }
    }
}