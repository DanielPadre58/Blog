using Blog.Application.Dtos.Comments;
using Blog.Application.Services.Comments;
using Blog.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController(ICommentService service) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ResponseModel<CommentDto>>> CreateComment([FromBody] CommentCreationDto dto)
    {
        var response = new ResponseModel<CommentDto>();
        var AuthorUsername = User.Identity?.Name;

        try
        {
            var comment = await service.CreateAsync(dto, AuthorUsername);
            response.SuccessResponse("Comment created successfully");
            return Ok(response);
        }
        catch (InvalidFieldsException ex)
        {
            return BadRequest(response.ErrorResponse(ex.Message));
        }
        catch (NotFoundException ex)
        {
            return NotFound(response.ErrorResponse(ex.Message));
        }
        catch (Exception e)
        {
            return StatusCode(500, response.ErrorResponse(e.Message));
        }
    }
}