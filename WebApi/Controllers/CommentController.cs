using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.DTO.Request;
using WebApi.Models.DTO.Response;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("/api/comments/")]
[Authorize]
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
    public IResult AddComment(AddCommentRequest addCommentRequest)
    {
        var email = HttpContext.User.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType).Value;
        var user = _userServiceService.GetUser(email);
        _commentServiceService.AddComment(addCommentRequest, user);
        return Results.Ok();
    }
}