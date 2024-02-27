using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.DTO.Response;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("/api/admin/")]
[Authorize(Roles = "admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    
    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }
    
    [HttpGet("comments")]
    public IEnumerable<CommentResponse> GetAllComments()
    {
        return _adminService.GetAllComments();
    }
    
    [HttpDelete("comments")]
    public IResult DeleteComment(int id)
    {
        _adminService.DeleteComment(id);
        return Results.Ok();
    }
    
    [HttpGet("films")]
    public IEnumerable<FilmResponse> GetAllFilms()
    {
        return _adminService.GetAllFilms();
    }
    
    [HttpPost("films")]
    public IResult AddFilm(FilmResponse filmResponse)
    {
        _adminService.AddFilm(filmResponse);
        return Results.Ok();
    }
    
    [HttpPut("films")]
    public IResult UpdateFilm(FilmResponse filmResponse)
    {
        _adminService.UpdateFilm(filmResponse);
        return Results.Ok();
    }
    
    [HttpDelete("films")]
    public IResult DeleteFilm(int id)
    {
        _adminService.DeleteFilm(id);
        return Results.Ok();
    }
}