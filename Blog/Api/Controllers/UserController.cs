using Blog.Application.Dtos.User;
using Blog.Application.Services.User;
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
        return service.Create(user)!.Result;
    }
}