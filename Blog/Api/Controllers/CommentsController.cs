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
        var authorUsername = User.Identity?.Name;

        try
        {
            await service.CreateAsync(dto, authorUsername!);
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
    
    [HttpDelete("{commentId}")]
    [Authorize]
    public async Task<ActionResult<ResponseModel<CommentDto>>> DeleteComment(int commentId)
    {
        var response = new ResponseModel<CommentDto>();
        var loggedUsername = User.Identity?.Name;

        try
        {
            await service.DeleteAsync(commentId, loggedUsername!);
            response.SuccessResponse("Comment deleted successfully");
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
    
    [HttpGet("{postId}")]
    [Authorize]
    public async Task<ActionResult<ResponseModel<List<CommentDto>>>> GetCommentsByPostId(int postId)
    {
        var response = new ResponseModel<List<CommentDto>>();
        var username = User.Identity?.Name;

        try
        {
            var comments = await service.GetByPostAsync(postId, username!);
            response.SuccessResponse("Comments retrieved successfully", comments);
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
        catch (Exception e)
        {
            return StatusCode(500, response.ErrorResponse(e.Message));
        }
    }
    
    [HttpGet("get/{parentId}")]
    [Authorize]
    public async Task<ActionResult<ResponseModel<List<CommentDto>>>> GetCommentsByParentId(int parentId)
    {
        var response = new ResponseModel<List<CommentDto>>();
        var username = User.Identity?.Name;

        try
        {
            var comments = await service.GetByParentAsync(parentId, username!);
            response.SuccessResponse("Comments retrieved successfully", comments);
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
        catch (Exception e)
        {
            return StatusCode(500, response.ErrorResponse(e.Message));
        }
    }
    
    [HttpPost("{id}/like")]
    [Authorize]
    public async Task<ActionResult<ResponseModel<CommentDto>>> LikeComment(int id)
    {
        var response = new ResponseModel<CommentDto>();
        var username = User.Identity?.Name;

        try
        {
            var comment = await service.LikeCommentAsync(id, username!);
            response.SuccessResponse("Post liked successfully", comment);
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
        catch (Exception ex)
        {
            return StatusCode(500, response.ErrorResponse(ex.Message));
        }
    }
        
    [HttpPost("{id}/dislike")]
    [Authorize]
    public async Task<ActionResult<ResponseModel<CommentDto>>> DislikeComment(int id)
    {
        var response = new ResponseModel<CommentDto>();
        var username = User.Identity?.Name;

        try
        {
            var comment = await service.DislikeCommentAsync(id, username!);
            response.SuccessResponse("Comment disliked successfully", comment);
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
        catch (Exception ex)
        {
            return StatusCode(500, response.ErrorResponse(ex.Message));
        }
    }
}