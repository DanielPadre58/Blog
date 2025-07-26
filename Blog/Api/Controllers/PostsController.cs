using Blog.Application.Dtos.Posts;
using Blog.Application.Services.Posts;
using Blog.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController(IPostService service) : ControllerBase
{
    [HttpGet("{id}")]
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