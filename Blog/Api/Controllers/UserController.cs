using Blog.Application.Dtos.User;
using Blog.Application.Services.Users;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService service)
{
    [HttpPost]
    public ResponseModel<User> CreateUser(UserCreationDto user)
    {
        return service.Create(user).Result;
    }
    
    [HttpDelete]
    public ResponseModel<User> DeleteUserById([FromQuery] int id)
    {
        return service.DeleteById(id).Result;
    }

    [HttpPut]
    public ResponseModel<User> EditUserById([FromQuery] int id, [FromBody] UserUpdateDto updatedUser)
    {
        return service.EditById(id, updatedUser).Result;
    }

    [HttpGet]
    public ResponseModel<User> GetUser([FromQuery] int? id, [FromQuery] string? username = null)
    {
        if (username != null && id.HasValue)
        {
            var response = new ResponseModel<User>
            {
                Status = System.Net.HttpStatusCode.BadRequest,
                Message = "Please provide either id or username, not both."
            };
            
            return response;
        }
        
        if(username == null && !id.HasValue)
        {
            var response = new ResponseModel<User>
            {
                Status = System.Net.HttpStatusCode.BadRequest,
                Message = "Either id or username must be provided."
            };
            
            return response;
        }
        
        return username != null ? service.GetByUsername(username).Result : service.GetById(id!.Value)!.Result;
    }
}