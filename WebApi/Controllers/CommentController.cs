using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.DTO.Request;
using WebApi.Models.DTO.Response;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("/api/comments/")]
public class CommentController : ControllerBase
{
    private readonly IUserService _userServiceService;
    private readonly ICommentService _commentServiceService;
    
    public CommentController(ICommentService commentServiceService, IUserService userServiceService)
    {
        _commentServiceService = commentServiceService;
        _userServiceService = userServiceService;
    }
    
    [HttpPost]
    [Authorize]
    public IResult AddComment(AddCommentRequest addCommentRequest)
    {
        var email = HttpContext.User.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType).Value;
        var user = _userServiceService.GetUser(email);
        _commentServiceService.AddComment(addCommentRequest, user);
        return Results.Ok();
    }
    
    [HttpGet]
    [Authorize(Roles = "admin")]
    public IEnumerable<CommentResponse> GetAllComments()
    {
        return _commentServiceService.GetAllComments();
    }
    
    [HttpDelete]
    [Authorize(Roles = "admin")]
    public IResult DeleteComment(int id)
    {
        _commentServiceService.DeleteComment(id);
        return Results.Ok();
    }
}