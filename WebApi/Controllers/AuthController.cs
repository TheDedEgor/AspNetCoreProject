using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.DTO.Request;
using WebApi.Models.DTO.Response;
using WebApi.Models.Entity;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authServiceService;
    
    public AuthController(IAuthService authServiceService)
    {
        _authServiceService = authServiceService;
    }
    
    [HttpPost("login")]
    public async Task<IResult> Login(LoginRequest loginRequest)
    {
        try
        {
            var account = _authServiceService.Login(loginRequest);
            await Authenticate(account);
            var userResponse = GetUserResponse(account);
            return Results.Ok(userResponse);
        }
        catch (BadHttpRequestException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
     
    [HttpPost("register")]
    public async Task<IResult> Register(RegisterRequest registerRequest)
    {
        try
        {
            var account = _authServiceService.Register(registerRequest);
            await Authenticate(account);
            var userResponse = GetUserResponse(account);
            return Results.Ok(userResponse);
        }
        catch (BadHttpRequestException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
    
    [HttpPost("logout")]
    public async Task<IResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Results.Ok();
    }
    
    private async Task Authenticate(Account account)
    {
        var claims = new List<Claim>
        {
            new (ClaimsIdentity.DefaultNameClaimType, account.Email),
            new (ClaimsIdentity.DefaultRoleClaimType, account.User.Role.Name)
        };
        
        var id = new ClaimsIdentity(claims, "ApplicationCookie", 
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }

    private static UserResponse GetUserResponse(Account account)
    {
        return new UserResponse(
            account.User.FirstName,
            account.User.LastName,
            account.User.Role.Name
        );
    }
}