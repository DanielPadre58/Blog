using Blog.Application.Dtos.Authentication;
using Blog.Application.Dtos.User;
using Blog.Application.Dtos.Users;
using Blog.Application.Services.Users;
using Blog.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ResponseModel<TokenDto>>> CreateUser(UserCreationDto user)
    {
        var response = new ResponseModel<TokenDto>();

        try
        {
            var username = await service.CreateAsync(user);
            response.SuccessResponse($"User with username {username} created successfully, check your email to verify your account");
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
            var tokens = await service.LoginAsync(loginData);
            response.SuccessResponse("Login successful", tokens);
            return Ok(response);
        }
        catch (InvalidAuthenticationData ex)
        {
            return BadRequest(response.ErrorResponse(ex.Message));
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
    
    [HttpPost("refresh")]
    public async Task<ActionResult<ResponseModel<TokenDto>>> RefreshToken([FromBody] RefreshTokenDto refreshToken)
    {
        var response = new ResponseModel<TokenDto>();

        try
        {
            var newToken = await service.RefreshAsync(refreshToken.refreshToken, refreshToken.username);
            response.SuccessResponse("Token refreshed successfully", newToken);
            return Ok(response);
        }
        catch (UnverifiedUserException ex)
        {
            return BadRequest(response.ErrorResponse(ex.Message));
        }
        catch (InvalidAuthenticationData ex)
        {
            return BadRequest(response.ErrorResponse(ex.Message));
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

    [HttpDelete("{username}")]
    [Authorize]
    public async Task<ActionResult<ResponseModel<UserDto>>> DeleteUser(string username)
    {
        var response = new ResponseModel<UserDto>();
        var loggedUsername = User.Identity?.Name;

        try
        {
            await service.DeleteAsync(username, loggedUsername);
            response.SuccessResponse("User deleted successfully");
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(response.ErrorResponse(ex.Message));
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
    [Authorize]
    public async Task<ActionResult<ResponseModel<UserDto>>> EditUser(string username, [FromBody] UserUpdateDto updatedUser)
    {
        var response = new ResponseModel<UserDto>();
        var loggedUsername = User.Identity?.Name;

        try
        {
            var user = await service.EditAsync(username, updatedUser, loggedUsername);
            response.SuccessResponse("User edited successfully", user);
            return Ok(response);
        }
        catch (InvalidFieldsException ex)
        {
            return BadRequest(response.ErrorResponse(ex.Message));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(response.ErrorResponse(ex.Message));
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
    [Authorize]
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
    public async Task<ActionResult<ResponseModel<TokenDto>>> VerifyUser(string validationCode)
    {
        var response = new ResponseModel<TokenDto>();

        try
        {
            var token = await service.VerifyUserAsync(validationCode);
            response.SuccessResponse("User verified successfully", token);
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