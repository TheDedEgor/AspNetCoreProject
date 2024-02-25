using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.DTO.Response;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("/api/users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userServiceService;
    
    public UserController(IUserService userServiceService)
    {
        _userServiceService = userServiceService;
    }
    
    [HttpGet]
    public UserDataResponse GetUser()
    {
        var email = HttpContext.User.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType).Value;
        var user = _userServiceService.GetUser(email);
        return new UserDataResponse(email, user.FirstName, user.LastName);
    }
}