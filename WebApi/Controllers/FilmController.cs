using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.DTO.Response;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("api/films")]
public class FilmController : ControllerBase
{
    private readonly IFilmService _filmServiceService;
    
    public FilmController(IFilmService filmServiceService)
    {
        _filmServiceService = filmServiceService;
    }
    
    [HttpGet]
    public IEnumerable<FilmShortDataResponse> GetFilms()
    { 
        return _filmServiceService.GetAllFilms();
    }
    
    [HttpGet("{id}")]
    [Authorize]
    public FilmResponse GetFilm(int id)
    {
        return _filmServiceService.GetFilm(id);
    }
    
    [HttpGet("search")]
    public IEnumerable<FilmShortDataResponse> SearchFilms(string text)
    { 
        return _filmServiceService.SearchFilms(text);
    }
}