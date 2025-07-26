using Blog.Application.Dtos.Posts;
using Blog.Application.Services.Posts;
using Blog.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController(IPostService service) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<PostDto>> CreatePost([FromBody] PostCreationDto dto)
    {
        var response = new ResponseModel<PostDto>();
        var authorUsername = User.Identity?.Name;

        try
        {
            var post = await service.CreateAsync(dto, authorUsername);
            response.SuccessResponse("Post created successfully");
            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, response);
        }
        catch (Exception e)
        {
            return StatusCode(500, response.ErrorResponse(e.Message));
        }
    }
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<ResponseModel<PostDto>>> GetPostById(int id)
    {
        var response = new ResponseModel<PostDto>();

        try
        {
            var post = await service.GetByIdAsync(id);
            response.SuccessResponse("Post retrieved successfully", post);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            
            return NotFound(response.ErrorResponse(ex.Message));
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
}